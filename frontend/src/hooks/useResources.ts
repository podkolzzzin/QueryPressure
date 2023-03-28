import { useEffect, useState } from 'react';
import i18n from 'i18next';

import { ResourcesApi } from '@/api';
import { ResourceModel } from '@/models/ResourceModel';

export function useResources() {
    const [resources, setResources] = useState<ResourceModel | null>(null);

    function loadResources(): void {
        ResourcesApi
            .getAll('en-US')
            .then(r => {
                setResources(r);
                i18n.addResources('en', 'translation', r);
            });
    }

    useEffect(() => {
        loadResources();
    }, []);

    return { resources };
}