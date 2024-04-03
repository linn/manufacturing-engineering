import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import { Grid } from '@mui/material';

import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';
import history from '../history';

function App() {
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h4">Manufacturing Engineering</Typography>
                </Grid>
                <Grid item xs={12}>
                    <List>
                        <ListItem component={Link} to="/manufacturing-engineering/inspections">
                            <Typography color="primary">Inspections Dashboard</Typography>
                        </ListItem>
                    </List>
                </Grid>
            </Grid>
        </Page>
    );
}

export default App;
