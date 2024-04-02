/* eslint-disable indent */
import React, { Fragment, useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import { Dropdown, InputField } from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import Page from './Page';
import config from '../config';
import history from '../history';
import useUserProfile from '../hooks/useUserProfile';
import useGet from '../hooks/useGet';
import itemTypes from '../itemTypes';

function Inspection({ creating }) {
    const { userNumber, name } = useUserProfile();
    const [orderNumber, setOrderNumber] = useState(827702);
    const { send: fetchPurchaseOrder, result: orderDetails } = useGet(
        itemTypes.purchaseOrderLine.url
    );

    const [inspectionData, setInspectionData] = useState({});

    useEffect(() => {
        if (orderDetails?.orderNumber) {
            setInspectionData(d => ({
                ...d,
                orderNumber: orderDetails.orderNumber,
                orderLine: orderDetails.orderLine
            }));
        }
    }, [orderDetails]);

    const onKeyPress = e => {
        if (e.key === 'Enter' && orderNumber) {
            fetchPurchaseOrder(null, `?orderNumber=${orderNumber}&lineNumber=${1}`);
        }
    };
    function createLinesArray(n) {
        const lines = [];
        for (let i = 0; i < n; i += 1) {
            const obj = {
                lineNumber: i + 1,
                status: 'PASSED',
                mottling: 'N',
                whiteSpot: 'N',
                chipped: 'N',
                marked: 'N',
                pitting: 'N',
                material: 'MATERIAL 1',
                timestamp: new Date()
            };
            lines.push(obj);
        }
        return lines;
    }

    const columns = [
        {
            field: 'lineNumber',
            headerName: '#',
            width: 100
        },
        {
            field: 'timestamp',
            headerName: 'Timestamp',
            type: 'dateTime',
            editable: true,
            width: 300
        },
        {
            field: 'status',
            headerName: 'Status',
            type: 'singleSelect',
            valueOptions: ['PASSED', 'PASSED AND REPAIRED', 'FAILED AND REPAIRED', 'FAIL'],
            editable: true,
            width: 250
        },
        {
            field: 'material',
            headerName: 'Material',
            type: 'singleSelect',
            valueOptions: ['MATERIAL 1', 'MATERIAL 2', 'etc'],
            editable: true,
            width: 150
        },
        {
            field: 'mottling',
            headerName: 'Mottling?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true,
            width: 150
        },
        {
            field: 'pitting',
            headerName: 'Pitting?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true,
            width: 150
        },
        {
            field: 'whiteSpot',
            headerName: 'White Spot?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true,
            width: 150
        },
        {
            field: 'chipped',
            headerName: 'Chipped?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true,
            width: 150
        },
        {
            field: 'marked',
            headerName: 'Marked?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N'],
            editable: true,
            width: 150
        }
    ];
    const processRowUpdate = newRow => {
        setInspectionData(d => ({
            ...d,
            lines: d.lines.map(x => (x.lineNumber === newRow.lineNumber ? { ...newRow } : x))
        }));

        return newRow;
    };

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <Typography variant="h4">
                        {creating ? 'Create New Inspection Record' : ''}
                    </Typography>
                </Grid>
                {userNumber && (
                    <>
                        <Grid item xs={4}>
                            <InputField
                                propertyName="searchTerm"
                                label="Purchase Order"
                                fullWidth
                                textFieldProps={{
                                    onKeyPress
                                }}
                                type="number"
                                helperText="enter an order number and press enter to load details"
                                value={orderDetails?.orderNumber ?? orderNumber}
                                onChange={(_, newVal) => setOrderNumber(newVal)}
                            />
                        </Grid>
                        <Grid item xs={4}>
                            <InputField
                                propertyName="enterdBy"
                                label="Entered By"
                                fullWidth
                                value={name}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item xs={4} />
                    </>
                )}
                {orderDetails && (
                    <>
                        <Grid item xs={4}>
                            <InputField
                                propertyName="partNumber"
                                label="Part"
                                fullWidth
                                value={orderDetails.partNumber}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <InputField
                                propertyName="partDescription"
                                label="Description"
                                fullWidth
                                value={orderDetails.partDescription}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                propertyName="qty"
                                label="Order Qty"
                                fullWidth
                                value={orderDetails.qty}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <InputField
                                disabled={inspectionData?.lines?.length}
                                propertyName="batchSize"
                                label="Batch Size"
                                fullWidth
                                helperText="Enter the size of the batch you are inspecting and then click proceed to initialise the table for data entry"
                                type="number"
                                value={inspectionData.batchSize}
                                onChange={(propertyName, newValue) => {
                                    setInspectionData(d => ({ ...d, [propertyName]: newValue }));
                                }}
                            />
                        </Grid>
                        <Grid item xs={2}>
                            <Dropdown
                                propertyName="preprocessedBatch"
                                label="Preprocessed Batch?"
                                fullWidth
                                value={inspectionData.preprocessedBatch}
                                onChange={(propertyName, newValue) => {
                                    setInspectionData(d => ({ ...d, [propertyName]: newValue }));
                                }}
                                items={['Y', 'N']}
                            />
                        </Grid>
                        {!inspectionData?.lines?.length && (
                            <Grid item xs={12}>
                                <Button
                                    onClick={() => {
                                        if (inspectionData.batchSize) {
                                            setInspectionData(d => ({
                                                ...d,
                                                lines: createLinesArray(inspectionData.batchSize)
                                            }));
                                        }
                                    }}
                                    variant="contained"
                                    color="primary"
                                >
                                    Proceed
                                </Button>
                            </Grid>
                        )}
                        <Grid item xs={12}>
                            <DataGrid
                                columns={columns}
                                autoHeight
                                columnBuffer={6}
                                processRowUpdate={processRowUpdate}
                                rows={
                                    inspectionData?.lines?.map(i => ({ ...i, id: i.lineNumber })) ||
                                    []
                                }
                            />
                        </Grid>
                    </>
                )}
            </Grid>
        </Page>
    );
}

Inspection.propTypes = { creating: PropTypes.bool };
Inspection.defaultProps = { creating: false };

export default Inspection;
