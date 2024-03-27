import { useState } from 'react';
import useGet from './useGet';

function useInitialise(url, id, queryString) {
    const [hasFetched, setHasFetched] = useState(false);
    const { send, isLoading, errorMessage, result } = useGet(url);

    if (!hasFetched) {
        send(id, queryString);
        setHasFetched(true);
    }
    return { isLoading, errorMessage, result };
}

export default useInitialise;
