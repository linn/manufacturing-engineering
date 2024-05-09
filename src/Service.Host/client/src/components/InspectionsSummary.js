import React from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import { Gauge, gaugeClasses } from '@mui/x-charts/Gauge';
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import { PieChart } from '@mui/x-charts/PieChart';
import { useDrawingArea } from '@mui/x-charts/hooks';
import { styled } from '@mui/material/styles';

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

    const averagePassPercentage = result?.length
        ? (
              result.reduce((acc, obj) => acc + parseFloat(obj.passPercentage), 0) / result.length
          ).toFixed(1)
        : 0;

    const data = [
        { value: 5, label: 'A' },
        { value: 10, label: 'B' },
        { value: 15, label: 'C' },
        { value: 20, label: 'D' }
    ];

    const size = {
        width: 380,
        height: 200
    };

    const StyledText = styled('text')(({ theme }) => ({
        fill: theme.palette.text.primary,
        textAnchor: 'middle',
        dominantBaseline: 'central',
        fontSize: 20
    }));

    function PieCenterLabel({ children }) {
        const { width, height, left, top } = useDrawingArea();
        return (
            <StyledText x={left + width / 2} y={top + height / 2}>
                {children}
            </StyledText>
        );
    }
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
                    </List>
                    <Grid item xs={12}>
                        <Stack direction={{ xs: 'column', md: 'row' }} spacing={{ xs: 1, md: 3 }}>
                            <Gauge
                                width={200}
                                height={200}
                                value={averagePassPercentage}
                                cornerRadius="50%"
                                text={({ value }) => `Avg Pass %: ${value}`}
                                sx={theme => ({
                                    [`& .${gaugeClasses.valueText}`]: {
                                        fontSize: 14
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
                                width={200}
                                height={200}
                                value={averagePassPercentage}
                                cornerRadius="50%"
                                text={({ value }) => `Anodised Pass %: ${value}`}
                                sx={theme => ({
                                    [`& .${gaugeClasses.valueText}`]: {
                                        fontSize: 14
                                    },
                                    [`& .${gaugeClasses.valueArc}`]: {
                                        fill: '#52b202'
                                    },
                                    [`& .${gaugeClasses.referenceArc}`]: {
                                        fill: theme.palette.text.disabled
                                    }
                                })}
                            />
                            <PieChart series={[{ data, innerRadius: 80 }]} {...size}>
                                <PieCenterLabel>Center label</PieCenterLabel>
                            </PieChart>
                        </Stack>
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
