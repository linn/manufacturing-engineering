import React, { useEffect } from 'react';
import { Route, Routes, Navigate, useLocation } from 'react-router-dom';
import { Loading } from '@linn-it/linn-form-components-library';
import { useAuth } from 'react-oidc-context';
import Navigation from '../containers/Navigation';
import useSignIn from '../hooks/useSignIn';
import LoggedOut from './LoggedOut';
import InspectionsSummary from './InspectionsSummary';
import Inspection from './Inspection';
import App from './App';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';

function Root() {
    const location = useLocation();
    const isLoggedOutRoute = location.pathname === '/manufacturing-engineering/logged-out';
    useSignIn({ disabled: isLoggedOutRoute });

    const auth = useAuth();

    useEffect(() => {
        const urlParams = new URLSearchParams(window.location.search);
        if (auth.isAuthenticated && urlParams.has('code') && urlParams.has('state')) {
            const cleanUrl = window.location.origin + window.location.pathname;
            window.history.replaceState({}, document.title, cleanUrl);
        }
    }, [auth.isAuthenticated]);

    const urlParams = new URLSearchParams(window.location.search);
    // More specific check for OIDC callback
    const isOidcCallback = urlParams.has('code') && urlParams.has('state');
    if (auth.isLoading || (isOidcCallback && !auth.isAuthenticated)) {
        return (
            <div>
                <div>
                    <Navigation />
                    <div
                        style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            height: 'calc(100vh - 80px)'
                        }}
                    >
                        <Loading />
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div>
            <div>
                <Navigation />
                <Routes>
                    <Route
                        exact
                        path="/"
                        element={<Navigate to="/manufacturing-engineering" replace />}
                    />
                    <Route path="/manufacturing-engineering" element={<App />} />
                    <Route
                        path="/manufacturing-engineering/inspections"
                        element={<InspectionsSummary />}
                    />
                    <Route
                        path="/manufacturing-engineering/inspections/create"
                        element={<Inspection creating />}
                    />
                    <Route
                        exact
                        path="/manufacturing-engineering/inspections/:id"
                        element={<Inspection />}
                    />
                    <Route element={<NotFoundPage />} />
                    <Route path="/manufacturing-engineering/logged-out" element={<LoggedOut />} />
                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </div>
        </div>
    );
}

export default Root;
