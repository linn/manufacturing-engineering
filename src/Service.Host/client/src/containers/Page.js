import { Page } from '@linn-it/linn-form-components-library';
import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Box from '@mui/material/Box';
import config from '../config';

function PageContainer({
    showBreadcrumbs = true,
    children,
    showRequestErrors = false,
    width = 'l',
    title = null,
    defaultAppTitle = 'service'
}) {
    const location = useLocation();
    const navigate = useNavigate();
    return (
        <Box sx={{ marginTop: '80px' }}>
            <Page
                homeUrl={config.appRoot}
                navigate={navigate}
                showBreadcrumbs={showBreadcrumbs}
                location={location}
                width={width}
                showRequestErrors={showRequestErrors}
                title={title}
                defaultAppTitle={defaultAppTitle}
            >
                {children}
            </Page>
        </Box>
    );
}

export default PageContainer;
