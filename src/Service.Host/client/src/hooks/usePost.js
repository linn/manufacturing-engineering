import { useState } from 'react';
import { useAuth } from 'react-oidc-context';

function usePost(url, id, data, requiresAuth = false) {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);
    const [postResult, setPostResult] = useState(null);

    let token = '';

    const auth = useAuth();
    if (requiresAuth) {
        token = auth.user?.access_token;
    }

    const send = async () => {
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
            setPostResult(await response.json());
            setIsLoading(false);
        } else {
            const text = await response.text();
            setErrorMessage(text);
            setIsLoading(false);
        }
    };

    return { send, isLoading, errorMessage, postResult };
}

export default usePost;
