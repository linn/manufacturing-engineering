import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import Page from './Page';
import config from '../config';
import history from '../history';

function PlaceHolderPage() {
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Typography variant="h4">Inspection</Typography>
            <List>
                <ListItem component={Link} to="/manufacturing-engineering">
                    <Typography color="primary">Home</Typography>
                </ListItem>
            </List>
        </Page>
    );
}

export default PlaceHolderPage;
