﻿import config from './config';

const itemTypes = {
    purchaseOrderLine: { url: `${config.appRoot}/manufacturing-engineering/purchase-orders` },
    inspections: { url: `${config.appRoot}/manufacturing-engineering/inspections` }
};

export default itemTypes;
