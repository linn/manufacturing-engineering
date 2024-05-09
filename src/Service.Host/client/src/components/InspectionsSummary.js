/* eslint-disable indent */
/* eslint-disable react/jsx-props-no-spreading */
import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import { Gauge, gaugeClasses } from '@mui/x-charts/Gauge';
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import { Loading, utilities } from '@linn-it/linn-form-components-library';
import { BarChart } from '@mui/x-charts/BarChart';
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
    const anodised = result?.filter(r => /\/A$|\/AB$/.test(r.partNumber));
    const averagePassPercentage = result?.length
        ? (
              result.reduce((acc, obj) => acc + parseFloat(obj.passPercentage), 0) / result.length
          ).toFixed(1)
        : 0;
    const averageAnodisedPassPercentage = anodised?.length
        ? (
              anodised.reduce((acc, obj) => acc + parseFloat(obj.passPercentage), 0) / result.length
          ).toFixed(1)
        : 0;
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
    const valueFormatter = value => `${value} Instances`;

    const getAnodisedFailureModeCount = mode => {
        const lines = anodised?.flatMap(obj => obj.lines);
        if (!lines?.length) {
            return 0;
        }
        return lines.reduce((acc, obj) => (obj[mode] === 'Y' ? acc + 1 : acc), 0);
    };

    const dataset = [
        {
            anodised: getAnodisedFailureModeCount('mottling'),
            mode: 'Mottling'
        },
        {
            anodised: getAnodisedFailureModeCount('pitting'),

            mode: 'Pitting'
        },
        {
            anodised: getAnodisedFailureModeCount('chipped'),

            mode: 'Chipped'
        },
        {
            anodised: getAnodisedFailureModeCount('marked'),

            mode: 'Marked'
        },
        {
            anodised: getAnodisedFailureModeCount('whiteSpot'),
            mode: 'White Spot'
        }
    ];
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12} style={{ paddingBottom: '40px' }}>
                    <Typography variant="h2">Inspection Summary</Typography>
                </Grid>
                <Grid item xs={12} style={{ paddingBottom: '40px' }}>
                    <Stack direction={{ xs: 'column', md: 'row' }} spacing={{ xs: 1, md: 3 }}>
                        <Gauge
                            width={250}
                            height={250}
                            value={averagePassPercentage}
                            cornerRadius="50%"
                            text={({ value }) => `Total Pass %: ${value}`}
                            sx={theme => ({
                                [`& .${gaugeClasses.valueText}`]: {
                                    fontSize: 18,
                                    fontFamily: 'roboto'
                                },
                                [`& .${gaugeClasses.valueArc}`]: {
                                    fill: '#52b202'
                                },
                                [`& .${gaugeClasses.referenceArc}`]: {
                                    fill: theme.palette.text.disabled
                                }
                            })}
                        />
                        <Gauge
                            width={250}
                            height={250}
                            value={averageAnodisedPassPercentage}
                            cornerRadius="50%"
                            text={({ value }) => `Anodised Pass %: ${value}`}
                            sx={theme => ({
                                [`& .${gaugeClasses.valueText}`]: {
                                    fontSize: 18,
                                    fontFamily: 'roboto'
                                },
                                [`& .${gaugeClasses.valueArc}`]: {
                                    fill: '#52b202'
                                },
                                [`& .${gaugeClasses.referenceArc}`]: {
                                    fill: theme.palette.text.disabled
                                }
                            })}
                        />
                        <BarChart
                            dataset={dataset}
                            yAxis={[{ scaleType: 'band', dataKey: 'mode' }]}
                            series={[
                                {
                                    dataKey: 'anodised',
                                    color: 'orange',
                                    label: 'Anodised Failure Breakdown',
                                    valueFormatter
                                }
                            ]}
                            layout="horizontal"
                            xAxis={[
                                {
                                    label: 'Number of Failure Mode Occurences'
                                }
                            ]}
                            width={400}
                            height={300}
                        />
                    </Stack>
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
                    </List>
                </Grid>
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
