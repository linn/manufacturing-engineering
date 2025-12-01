import { useState, useRef, useCallback } from 'react';
import { useAuth } from 'react-oidc-context';
import { useNavigate } from 'react-router-dom';
import { utilities } from '@linn-it/linn-form-components-library';

// this hook is just a placeholder for a show & tell
// on how you might force your client to get a freshly minted access token
// just before it makes a POST, as opposed to just using whatever token
// was last stored in local storage, which could be out of date
// with regards to users permissions if changes have been made recently
function usePost(url, requiresAuth = false, redirectOnSuccess = false) {
    const [isLoading, setIsLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState(null);
    const [postResult, setPostResult] = useState(null);

    const navigate = useNavigate();
    const abortControllerRef = useRef(null);

    const auth = useAuth();

    const clearPostResult = useCallback(() => {
        setPostResult(null);
        setErrorMessage(null);
    }, []);

    const send = useCallback(
        async (id, data) => {
            if (abortControllerRef.current) {
                abortControllerRef.current.abort();
            }

            const controller = new AbortController();
            abortControllerRef.current = controller;

            setIsLoading(true);
            setPostResult(null);
            setErrorMessage(null);

            let token = '';
            if (requiresAuth && auth && auth.signinSilent) {
                try {
                    const user = await auth.signinSilent();
                    token = user?.access_token;
                    console.log(token);
                } catch (err) {
                    setErrorMessage(`Failed to obtain fresh token: ${err.message}`);
                    setIsLoading(false);
                    return;
                }
            } else if (requiresAuth) {
                token = auth.user?.access_token;
            }

            const headers = {
                accept: 'application/json',
                'Content-Type': 'application/json'
            };

            const requestParameters = {
                method: 'POST',
                body: JSON.stringify(data),
                headers: requiresAuth ? { ...headers, Authorization: `Bearer ${token}` } : headers,
                signal: controller.signal
            };

            try {
                const response = await fetch(id ? `${url}/${id}` : url, requestParameters);

                if (response.ok) {
                    const result = await response.json();
                    setPostResult(result);
                    if (redirectOnSuccess) {
                        setPostResult(null);
                        navigate(utilities.getSelfHref(result));
                    }
                } else {
                    const text = await response.text();
                    setErrorMessage(text);
                }
            } catch (error) {
                if (error.name === 'AbortError') {
                    console.log('Request was aborted.');
                } else {
                    setErrorMessage('An error occurred.');
                }
            } finally {
                setIsLoading(false);
            }
        },
        [requiresAuth, url, redirectOnSuccess, navigate, auth]
    );

    const cancelRequest = useCallback(() => {
        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }
    }, []);

    return { send, cancelRequest, isLoading, errorMessage, postResult, clearPostResult };
}

export default usePost;
