import {useState} from 'react';

export function useToggle(): [boolean, (enabled?: boolean) => void] {
  const [state, setState] = useState<boolean>(false);

  function toogle(newState?: boolean): void {
    if (newState === true)
        return setState(true);

    if (newState === false)
        return setState(false);

    setState(!state)
  }

  return [ state, toogle ];
}
