﻿import React from 'react';
import { Route, Routes } from 'react-router';
import { Navigate, unstable_HistoryRouter as HistoryRouter } from 'react-router-dom';
import App from './App';
import 'typeface-roboto';
import NotFoundPage from './NotFoundPage';
import history from '../history';
import useSignIn from '../hooks/useSignIn';
import Navigation from '../containers/Navigation';
import InspectionsSummary from './InspectionsSummary';
import Inspection from './Inspection';

function Root() {
    useSignIn();
    return (
        <div>
            <div className="padding-top-when-not-printing">
                <Navigation />
                <HistoryRouter history={history}>
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
                    </Routes>
                </HistoryRouter>
            </div>
        </div>
    );
}

export default Root;
