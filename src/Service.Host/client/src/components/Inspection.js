import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import { DataGrid } from '@mui/x-data-grid';
import Button from '@mui/material/Button';
import { useParams } from 'react-router-dom';
import {
    Dropdown,
    InputField,
    Loading,
    SaveBackCancelButtons,
    SnackbarMessage,
    usePost,
    usePut,
    useGet
} from '@linn-it/linn-form-components-library';
import PropTypes from 'prop-types';
import moment from 'moment';
import Page from '../containers/Page';
import config from '../config';
import useUserProfile from '../hooks/useUserProfile';
import itemTypes from '../itemTypes';

function Inspection({ creating }) {
    const { id } = useParams();
    const { userNumber, name } = useUserProfile();
    const [orderNumber, setOrderNumber] = useState();
    const [inspectionData, setInspectionData] = useState({ preprocessedBatch: 'N' });
    const [changesMade, setChangesMade] = useState(false);

    const {
        send: fetchInspection,
        result: inspectionDetails,
        isLoading: inspectionLoading
    } = useGet(itemTypes.inspections.url);

    const {
        send: fetchPurchaseOrder,
        result: orderDetails,
        clearData: clearOrderDetails
    } = useGet(itemTypes.purchaseOrderLine.url, false);

    const {
        send: post,
        isLoading: postLoading,
        postResult,
        clearPostResult
    } = usePost(itemTypes.inspections.url, true, true);

    const {
        send: put,
        isLoading: putLoading,
        putResult,
        clearPutResult
    } = usePut(itemTypes.inspections.url, true);

    const [hasFetched, setHasFetched] = useState(false);

    if (id && !hasFetched) {
        setHasFetched(true);
        fetchInspection(id);
    }

    useEffect(() => {
        if (inspectionDetails?.id) {
            setInspectionData(inspectionDetails);
        }
    }, [inspectionDetails]);

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
        if (creating && e.key === 'Enter' && orderNumber) {
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
                material: 'Not Specified',
                sentToReprocess: '',
                timestamp: null
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
            headerName: 'Machining Timestamp',
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
            valueOptions: [
                'Not Specified',
                'Gleich 5083',
                'Gleich 6082',
                'V-Cast 5754',
                'Alimex 5754'
            ],
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

    if (inspectionData?.preprocessedBatch === 'N') {
        columns.push({
            field: 'sentToReprocess',
            headerName: 'Sent To Reprocess?',
            type: 'singleSelect',
            valueOptions: ['Y', 'N', ''],
            editable: true,
            width: 200
        });
    }

    const processRowUpdate = newRow => {
        setChangesMade(true);
        setInspectionData(d => ({
            ...d,
            lines: d.lines.map(x => (x.lineNumber === newRow.lineNumber ? { ...newRow } : x))
        }));

        return newRow;
    };

    if (inspectionLoading || postLoading || putLoading) {
        return (
            <Page homeUrl={config.appRoot} history={history}>
                <Grid container spacing={3}>
                    <Grid item size={12}>
                        <Loading />
                    </Grid>
                </Grid>
            </Page>
        );
    }

    return (
        <Page homeUrl={config.appRoot} history={history}>
            <SnackbarMessage
                visible={!!postResult?.id || !!putResult?.id}
                onClose={() => {
                    clearPostResult();
                    clearPutResult();
                }}
                message="Save Successful"
            />
            <Grid container spacing={3}>
                <Grid item size={12}>
                    <Typography variant="h4">
                        {creating ? 'Create New Inspection Record' : 'Inspection Record Details'}
                    </Typography>
                </Grid>
                {userNumber && (
                    <>
                        <Grid item size={4}>
                            <InputField
                                propertyName="searchTerm"
                                label="Purchase Order"
                                disabled={!creating}
                                fullWidth
                                textFieldProps={{
                                    onKeyPress
                                }}
                                type="number"
                                helperText="enter an order number and press enter to load details"
                                value={creating ? orderNumber : inspectionData?.orderNumber}
                                onChange={(_, newVal) => {
                                    if (creating) {
                                        setOrderNumber(newVal);
                                    }
                                }}
                            />
                        </Grid>
                        <Grid item size={4}>
                            <InputField
                                propertyName="enterdBy"
                                label="Entered By"
                                fullWidth
                                value={creating ? name : inspectionData.enteredByName}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item size={4}>
                            <InputField
                                propertyName="dateOfEntry"
                                label="Date Of Entry"
                                fullWidth
                                value={moment(inspectionData.dateOfEntry).format('DD MMM YYYY')}
                                onChange={() => {}}
                            />
                        </Grid>
                    </>
                )}
                {(orderDetails || inspectionData?.id) && (
                    <>
                        <Grid item size={4}>
                            <InputField
                                propertyName="partNumber"
                                label="Part"
                                fullWidth
                                value={orderDetails?.partNumber ?? inspectionData?.partNumber}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item size={6}>
                            <InputField
                                propertyName="partDescription"
                                label="Description"
                                fullWidth
                                value={
                                    orderDetails?.partDescription ?? inspectionData?.partDescription
                                }
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item size={2}>
                            <InputField
                                propertyName="qty"
                                label="Order Qty"
                                fullWidth
                                value={orderDetails?.qty ?? inspectionData?.orderQty}
                                onChange={() => {}}
                            />
                        </Grid>
                        <Grid item size={2}>
                            <InputField
                                disabled={!creating || inspectionData?.lines?.length}
                                propertyName="batchSize"
                                label="Batch Size"
                                fullWidth
                                helperText={
                                    creating
                                        ? 'Enter the size of the batch you are inspecting and then click proceed to initialise the table for data entry'
                                        : ''
                                }
                                type="number"
                                value={inspectionData?.batchSize}
                                onChange={(propertyName, newValue) => {
                                    if (creating) {
                                        setChangesMade(true);
                                        setInspectionData(d => ({
                                            ...d,
                                            [propertyName]: newValue
                                        }));
                                    }
                                }}
                            />
                        </Grid>
                        <Grid item size={2}>
                            <Dropdown
                                propertyName="preprocessedBatch"
                                label="Preprocessed Batch?"
                                fullWidth
                                value={inspectionData?.preprocessedBatch}
                                onChange={(propertyName, newValue) => {
                                    setChangesMade(true);
                                    setInspectionData(d => ({
                                        ...d,
                                        [propertyName]: newValue
                                    }));
                                }}
                                items={['Y', 'N']}
                            />
                        </Grid>
                        {creating && !inspectionData?.lines?.length && (
                            <Grid item size={12}>
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
                        <Grid item size={12}>
                            {(!creating || inspectionData?.lines?.length) && (
                                <DataGrid
                                    columns={columns}
                                    autoHeight
                                    columnBuffer={6}
                                    disableRowSelectionOnClick
                                    processRowUpdate={processRowUpdate}
                                    rows={
                                        inspectionData?.lines?.map(i => ({
                                            ...i,
                                            id: i.lineNumber,
                                            timestamp: i.timestamp ? new Date(i.timestamp) : null
                                        })) || []
                                    }
                                />
                            )}
                        </Grid>
                        <Grid item size={12}>
                            <SaveBackCancelButtons
                                cancelClick={() => {
                                    setInspectionData({ preprocessedBatch: 'N' });
                                    clearOrderDetails();
                                    setChangesMade(false);
                                }}
                                saveClick={() => {
                                    setChangesMade(false);
                                    clearPostResult();
                                    clearPutResult();
                                    if (creating) {
                                        post(null, inspectionData);
                                    } else {
                                        put(id, inspectionData);
                                    }
                                }}
                                saveDisabled={!inspectionData?.lines?.length || !changesMade}
                                backClick={() =>
                                    history.push('/manufacturing-engineering/inspections')
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
