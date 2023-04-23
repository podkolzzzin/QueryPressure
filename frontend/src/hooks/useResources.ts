import i18n from 'i18next';

import { ResourcesApi } from '@/api';
import { useTranslation } from 'react-i18next';
import { useEffect, useState } from 'react';
import { LocalStorageService } from '@/services';
import { LocalesApi } from '@/api/LocalesApi';

type TLanguages = { [key: string]: string; };

const DEFAULT_LANGUAGE = 'US';
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

export async function loadResources(newLng: string) {
    return await ResourcesApi
        .getAll(newLng)
        .then(r => {
            if (r) {
                i18n.addResourceBundle(newLng, 'translation', r);
            } else {
                console.error('No such resource!');
            }
            return r;
        });
};

export function useResources() {
    const { i18n } = useTranslation();
    const [language, setLanguage] = useState<string>(DEFAULT_LANGUAGE);
    const [alreadyLoaded, setAlreadyLoaded] = useState<string[]>([]);
    const [locales, setLocales] = useState<TLanguages>();
    const languages: string[] = locales ? Object.keys(locales) : [];


    const changeLanguage = async (lngName: string, initLocales?: TLanguages) => {
        const newLngCode = initLocales?.[lngName] || locales?.[lngName];

        if (!newLngCode || lngName === language) {
            return;
        };

        const newLngName = lngName;

        const saveNewLanguage = () => {
            LocalStorageService.set(LS_LANGUAGE_KEY, lngName);
            setLanguage(newLngName);
        };

        if (alreadyLoaded.includes(newLngName)) {
            i18n.changeLanguage(newLngCode);
            saveNewLanguage();
        } else {
            const res = await loadResources(newLngCode);
            if (res) {
                i18n.changeLanguage(newLngCode);
                setAlreadyLoaded(prev => ([...prev, newLngName]));
                saveNewLanguage();
            }
        };
    };

    useEffect(() => {
        (async function () {
            const lsLng = LocalStorageService.get<string>(LS_LANGUAGE_KEY);
            const localesRes = await loadLocales();
            setLocales(localesRes);

            if (lsLng && localesRes[lsLng] && lsLng !== DEFAULT_LANGUAGE) {
                changeLanguage(lsLng, localesRes);
            } else {
                loadResources(localesRes[DEFAULT_LANGUAGE]);
                setAlreadyLoaded([DEFAULT_LANGUAGE])
            };
        })();
    }, []);

    return { languages, language, changeLanguage };
}
