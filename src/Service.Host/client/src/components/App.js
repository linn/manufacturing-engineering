import React from 'react';
import Typography from '@mui/material/Typography';
import Page from './Page';
import config from '../config';
import history from '../history';

function App() {
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Typography variant="h6">Manufacturing Engineering</Typography>
        </Page>
    );
}

export default App;
