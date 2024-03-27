import { useEffect, useState } from 'react';
import { useAuth, hasAuthParams } from 'react-oidc-context';

function useSignIn() {
    const auth = useAuth();

    const [hasTriedSignin, setHasTriedSignin] = useState(false);

    // automatically sign-in
    useEffect(() => {
        if (
            !hasAuthParams() &&
            !auth.isAuthenticated &&
            !auth.activeNavigator &&
            !auth.isLoading &&
            !hasTriedSignin
        ) {
            auth.signinRedirect();
            setHasTriedSignin(true);
        }
    }, [auth, hasTriedSignin]);
}

export default useSignIn;
