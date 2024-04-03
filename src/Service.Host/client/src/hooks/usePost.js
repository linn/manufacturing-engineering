import { useState } from 'react';
import { utilities } from '@linn-it/linn-form-components-library';
import { useAuth } from 'react-oidc-context';
import history from '../history';

function usePost(url, requiresAuth = false, redirectOnSuccess = false) {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);
    const [postResult, setPostResult] = useState(null);

    let token = '';

    const auth = useAuth();
    if (requiresAuth) {
        token = auth.user?.access_token;
    }

    const clearResult = () => setPostResult(null);

    const send = async (id, data) => {
        setIsLoading(true);
        setPostResult(null);
        setErrorMessage(null);

        const headers = {
            accept: 'application/json',
            'Content-Type': 'application/json'
        };
        const requestParameters = {
            method: 'POST',
            body: JSON.stringify(data),
            headers: requiresAuth ? { ...headers, Authorization: `Bearer ${token}` } : headers
        };

        const response = await fetch(id ? `${url}/${id}` : url, requestParameters);

        if (response.ok) {
            const result = await response.json();
            setPostResult(result);
            setIsLoading(false);
            if (redirectOnSuccess) {
                // redirect to the rel:self link of the result
                history.push(utilities.getSelfHref(result));
            }
        } else {
            const text = await response.text();
            setErrorMessage(text);
            setIsLoading(false);
        }
    };

    return { send, isLoading, errorMessage, postResult, clearResult };
}

export default usePost;
