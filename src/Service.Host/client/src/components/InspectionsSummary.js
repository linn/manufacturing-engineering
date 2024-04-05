import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import { Grid } from '@mui/material';
import { Loading, utilities } from '@linn-it/linn-form-components-library';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import moment from 'moment';
import { DataGrid } from '@mui/x-data-grid';
import Page from './Page';
import config from '../config';
import history from '../history';
import useInitialise from '../hooks/useInitialise';
import itemTypes from '../itemTypes';

function InspectionsSummary() {
    const { result, isLoading } = useInitialise(itemTypes.inspections.url);

    const columns = [
        {
            field: 'dateOfEntry',
            headerName: 'Inspection Date',
            width: 200,
            valueGetter: ({ value }) => value && moment(value).format('DD MMM YYYY')
        },
        {
            field: 'partNumber',
            headerName: 'Part',
            width: 200
        },
        {
            field: 'batchSize',
            headerName: 'Batch Size',
            width: 200
        },
        {
            field: 'passPercentage',
            headerName: 'Pass Percentage',
            align: 'right',
            width: 200
        },
        {
            field: 'enteredByName',
            headerName: 'Inspected By',
            width: 200
        },
        {
            field: 'preprocessedBatch',
            headerName: 'Preprocessed Batch',
            width: 200
        }
    ];
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h4">Inspection</Typography>
                </Grid>
                <Grid item xs={12}>
                    <List>
                        <ListItem
                            component={Link}
                            to="/manufacturing-engineering/inspections/create"
                        >
                            <Typography variant="h6" color="primary">
                                Create New Inspection Record
                            </Typography>
                        </ListItem>
                        <ListItem>
                            <Typography variant="subtitle2">
                                Click the link above to log a new batch inspection.
                            </Typography>
                        </ListItem>
                        <ListItem>
                            <Typography variant="subtitle2">
                                Recently created records will be listed below for now, until we
                                implement some real reporting. You can click through to view
                                details.
                            </Typography>
                        </ListItem>
                    </List>
                    <Grid item xs={12}>
                        {result && (
                            <DataGrid
                                columns={columns}
                                autoHeight
                                columnBuffer={6}
                                rows={result}
                                onRowClick={clicked => {
                                    history.push(utilities.getSelfHref(clicked.row));
                                }}
                            />
                        )}
                    </Grid>
                </Grid>
                {isLoading && (
                    <Grid item xs={12}>
                        <Loading />
                    </Grid>
                )}
            </Grid>
        </Page>
    );
}

export default InspectionsSummary;
