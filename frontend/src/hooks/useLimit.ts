import {useEffect, useState} from 'react';
import {LimitsApi} from '@api';
import {LimitModel} from '@models';

export function useLimit() {
  const [limits, setLimits] = useState<LimitModel[]>([]);
  const [selectedLimit, setSelectedLimit] = useState<LimitModel | null>(null);

  function selectLimit(limitType: string) {
    const limit: LimitModel = limits.find(p => p.type === limitType)!;
    setSelectedLimit(limit);
  }

  function loadLimits(): void {
    LimitsApi
      .getAll()
      .then(limits => setLimits(limits));
  }

  useEffect(() => {
    loadLimits();
  }, [setLimits]);

  return {
    limits,
    selectedLimit,
    selectLimit
  };
}
