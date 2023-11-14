import { useTranslation } from 'react-i18next';
import { useEffect, useState } from 'react';
import { LocalStorageService } from '@/services';
import { LocalesApi } from '@/api/LocalesApi';

type TLanguages = { [key: string]: string; };

const LS_LANGUAGE_KEY = 'locale';

async function loadLocales() {
    return await LocalesApi
        .get()
        .then(r => {
            return r.reduce((res, item) => {
                const name = item.split('-')[1];
                const code = item;

                res[name] = code;
                return res;
            }, {} as TLanguages);
        })
};

export function useResources() {
    const lastOrDefaultLang = LocalStorageService.get<string>(LS_LANGUAGE_KEY) ?? 'en-US';
    const { i18n } = useTranslation();
    const [language, setLanguage] = useState<string>(lastOrDefaultLang);
    const [locales, setLocales] = useState<TLanguages>();

    const changeLanguage = async (lngName: string, initLocales?: TLanguages) => {
        const newLngCode = initLocales?.[lngName] || locales?.[lngName];

        if (!newLngCode) {
            return;
        };

        await i18n.changeLanguage(newLngCode);
        LocalStorageService.set(LS_LANGUAGE_KEY, lngName);
        setLanguage(lngName);
    };

    useEffect(() => {
        (async function () {
            const lsLng = LocalStorageService.get<string>(LS_LANGUAGE_KEY);
            const localesRes = await loadLocales();
            setLocales(localesRes);

            if (lsLng && localesRes[lsLng]) {
                changeLanguage(lsLng, localesRes);
            }
        })();
    }, []);

    return { 
        get languages() {
            if (locales)
                return Object.keys(locales);
            
            return [];
        }, 
        language, 
        changeLanguage };
}
