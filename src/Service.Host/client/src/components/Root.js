import React from 'react';
import { Route, Routes } from 'react-router';
import { Navigate, unstable_HistoryRouter as HistoryRouter } from 'react-router-dom';
import App from './App';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';
import history from '../history';
import useSignIn from '../hooks/useSignIn';

function Root() {
    useSignIn();
    return (
        <div>
            <div className="padding-top-when-not-printing">
                <HistoryRouter history={history}>
                    <Routes>
                        <Route
                            exact
                            path="/"
                            element={<Navigate to="/manufacturing-engineering" replace />}
                        />
                        <Route path="/manufacturing-engineering" element={<App />} />
                        <Route exact path="/manufacturing-engineering/:id" element={<App />} />

                        <Route element={<NotFoundPage />} />
                    </Routes>
                </HistoryRouter>
            </div>
        </div>
    );
}

export default Root;
