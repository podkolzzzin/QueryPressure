import {useEffect, useState} from 'react';
import {LimitsApi} from '@api';
import {LimitModel} from '@models';

import { UrlService } from '@/services/UrlService';

export function useLimit() {
  const [limits, setLimits] = useState<LimitModel[]>([]);
  const [selectedLimit, setSelectedLimit] = useState<LimitModel | null>(null);

  function selectLimit(limitType: string) {
    const limit: LimitModel = limits.find(p => p.type === limitType)!;
    setSelectedLimit(limit);
    UrlService.set('limit', limitType);
  }

  function loadLimits(): void {
    LimitsApi
      .getAll()
      .then(limits => {
         setLimits(limits);
        // After the limits have been loaded, check if a 'limit' parameter exists in the URL
        const urlLimitType = UrlService.get('limit');
        // If it does exist, select the limit with that type
        if (urlLimitType) {
          selectLimit(urlLimitType);
        }
      });
  }

  useEffect(() => {
    loadLimits();
  });

  return {
    limits,
    selectedLimit,
    selectLimit
  };
}
