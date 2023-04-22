import i18n from 'i18next';

import { ResourcesApi } from '@/api';
import { useTranslation } from 'react-i18next';
import { useEffect, useState } from 'react';
import { LocalStorageService } from '@/services';

type TLanguages = 'EN' | 'UA';

const LANGUAGES: { [key in TLanguages]: string; } = {
    'EN': 'en-US',
    'UA': 'uk-UA',
};
const DEFAULT_LANGUAGE = 'EN';
const LS_LANGUAGE_KEY = 'locale';

export async function loadResources(newLng: string) {
    return await ResourcesApi
        .getAll(newLng)
        .then(r => {
            if(r){
                i18n.addResourceBundle(newLng, 'translation', r);
            }else{
                console.error('No such resource!');
            }
            return r;
        });
};

export function useResources() {
    const { i18n } = useTranslation();
    const languages = Object.keys(LANGUAGES) as TLanguages[];
    const [language, setLanguage] = useState<TLanguages>(DEFAULT_LANGUAGE);
    const [ alreadyLoaded, setAlreadyLoaded] = useState<TLanguages[]>([]);
    
    
    const changeLanguage = async (lngName: string) => {
        const newLngCode = LANGUAGES[lngName as TLanguages];

        if(!newLngCode || lngName === language){
            return;
        };

        const newLngName = lngName as TLanguages;
        
        const saveNewLanguage = () => {
            LocalStorageService.set(LS_LANGUAGE_KEY, lngName);
            setLanguage(newLngName);
        };

        if(alreadyLoaded.includes(newLngName)){
            i18n.changeLanguage(newLngCode);
            saveNewLanguage();
        }else{
            const res = await loadResources(newLngCode);
            if(res){
                i18n.changeLanguage(newLngCode);
                setAlreadyLoaded(prev => ([...prev, newLngName]));
                saveNewLanguage();
            }
        };
    };

    useEffect(() => {
        const lsLng = LocalStorageService.get<TLanguages>(LS_LANGUAGE_KEY);
        
        if(lsLng && LANGUAGES[lsLng] && lsLng !== DEFAULT_LANGUAGE){
            changeLanguage(lsLng)
        }else{
            loadResources(LANGUAGES[DEFAULT_LANGUAGE]);
            setAlreadyLoaded([DEFAULT_LANGUAGE])
        };
    }, []);

    return { languages, language, changeLanguage };
}