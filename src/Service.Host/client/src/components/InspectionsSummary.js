/* eslint-disable indent */
/* eslint-disable react/jsx-props-no-spreading */
import React, { useState } from 'react';
import Typography from '@mui/material/Typography';
import { Link } from 'react-router-dom';
import { Gauge, gaugeClasses } from '@mui/x-charts/Gauge';
import Grid from '@mui/material/Grid';
import Stack from '@mui/material/Stack';
import { Dropdown, Loading, utilities, DatePicker } from '@linn-it/linn-form-components-library';
import { BarChart } from '@mui/x-charts/BarChart';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import moment from 'moment';
import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import Page from './Page';
import config from '../config';
import history from '../history';
import useInitialise from '../hooks/useInitialise';
import itemTypes from '../itemTypes';

function InspectionsSummary() {
    const { result, isLoading } = useInitialise(itemTypes.inspections.url);
    const [filters, setFilters] = useState({ part: 'ALL', fromDate: null, toDate: null });
    let filtered = [];

    if (filters.part === 'ALL') {
        filtered = result;
    } else if (filters.part === 'ALL ANODISED') {
        filtered = result?.filter(r => /\/A$|\/AB$/.test(r.partNumber));
    } else {
        filtered = result?.filter(x => x.partNumber === filters.part);
    }

    const partFilterOptions = [
        ...new Set(['ALL', 'ALL ANODISED', ...(result ? result.map(r => r.partNumber) : [])])
    ];

    if (filters.fromDate) {
        filtered = filtered.filter(r =>
            moment(r.dateOfEntry).isSameOrAfter(moment(filters.fromDate).startOf('day'))
        );
    }

    if (filters.toDate) {
        filtered = filtered.filter(r =>
            moment(r.dateOfEntry).isSameOrBefore(moment(filters.toDate).startOf('day'))
        );
    }

    const averagePassPercentage = filtered?.length
        ? (
              filtered.reduce((acc, obj) => acc + parseFloat(obj.passPercentage), 0) /
              filtered.length
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
        },
        {
            field: 'supplierName',
            headerName: 'Supplier',
            width: 300
        }
    ];
    const valueFormatter = value => `${value} Instances`;

    const getFailureModeCount = mode => {
        const lines = filtered?.flatMap(obj => obj.lines);
        if (!lines?.length) {
            return 0;
        }
        return lines.reduce((acc, obj) => (obj[mode] === 'Y' ? acc + 1 : acc), 0);
    };

    const dataset = [
        {
            anodised: getFailureModeCount('mottling'),
            mode: 'Mottling'
        },
        {
            anodised: getFailureModeCount('pitting'),

            mode: 'Pitting'
        },
        {
            anodised: getFailureModeCount('chipped'),

            mode: 'Chipped'
        },
        {
            anodised: getFailureModeCount('marked'),

            mode: 'Marked'
        },
        {
            anodised: getFailureModeCount('whiteSpot'),
            mode: 'White Spot'
        }
    ];
    const getFillColour = passPercentage => {
        switch (true) {
            //  red when below 75%, orange/yellow between 75-80 and green above 80.
            case passPercentage < 75:
                return '#ff4848';
            case passPercentage >= 75 && passPercentage <= 80:
                return '#ffa500';
            case passPercentage > 80:
                return '#52b202';
            default:
                return '#000000';
        }
    };
    const handleFilterChange = (propertyName, newValue) => {
        setFilters(f => ({ ...f, [propertyName]: newValue }));
    };
    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h2">Inspection Summary</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle2">
                        You can adjust the filters below to change the visualisation
                    </Typography>
                </Grid>
                <Grid item xs={3} style={{ paddingBottom: '40px' }}>
                    <Dropdown
                        allowNoValue={false}
                        items={partFilterOptions}
                        propertyName="part"
                        onChange={handleFilterChange}
                        label="PART FILTER"
                        value={filters.part}
                    />
                </Grid>
                <Grid item xs={3} style={{ paddingBottom: '40px' }}>
                    <DatePicker
                        label="From Date"
                        value={filters.fromDate}
                        onChange={newVal => handleFilterChange('fromDate', newVal)}
                    />
                </Grid>
                <Grid item xs={3} style={{ paddingBottom: '40px' }}>
                    <DatePicker
                        label="To Date"
                        value={filters.toDate}
                        onChange={newVal => handleFilterChange('toDate', newVal)}
                    />
                </Grid>

                <Grid item xs={12} style={{ paddingBottom: '40px' }}>
                    <Stack direction={{ xs: 'column', md: 'row' }} spacing={{ xs: 1, md: 3 }}>
                        <Gauge
                            width={350}
                            height={350}
                            value={averagePassPercentage}
                            cornerRadius="50%"
                            text={({ value }) => `Pass %: ${value}`}
                            sx={theme => ({
                                [`& .${gaugeClasses.valueText}`]: {
                                    fontSize: 18,
                                    fontFamily: 'roboto'
                                },
                                [`& .${gaugeClasses.valueArc}`]: {
                                    fill: getFillColour(averagePassPercentage)
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
                                    label: 'Failure Modes Breakdown For Selected Parameters',
                                    valueFormatter
                                }
                            ]}
                            layout="horizontal"
                            xAxis={[
                                {
                                    label: 'Number of Failure Mode Occurences'
                                }
                            ]}
                            width={500}
                            height={350}
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
                                Click the link above to log a new batch inspection. Click a record
                                below to view/edit it. You can filter and sort the table below, or
                                click export to download a spreadsheet.
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
                            slots={{ toolbar: GridToolbar }}
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
