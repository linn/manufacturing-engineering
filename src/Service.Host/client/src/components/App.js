import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';
import history from '../history';

function App() {
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Typography variant="h4">Manufacturing Engineering</Typography>
            <List>
                <ListItem component={Link} to="/manufacturing-engineering/inspection">
                    <Typography color="primary">Inspection</Typography>
                </ListItem>
            </List>
        </Page>
    );
}

export default App;
