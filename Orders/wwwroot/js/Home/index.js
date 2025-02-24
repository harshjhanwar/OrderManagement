$(function () {
    $("#gridContainer").dxDataGrid({
        dataSource: {
            load: function () {
                return $.ajax({
                    url: "/api/Orders",
                    dataType: "json"
                }).then(function (response) {
                    if (!response.success) {
                        console.log(response.message);
                        alert(response.message);
                        return;
                    }
                    return response.data;
                }).fail(function (jqXHR) {
                    console.log("Error occurred in load operation");
                });
            },
            insert: function (values) {
                console.log(values);
                return $.ajax({
                    url: "/api/Orders",
                    method: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(values)
                }).then(function (response) {
                    if (!response.success) {
                        console.log(response.message);
                        alert(response.message);
                        return;
                    }
                }).fail(function (jqXHR) {
                    console.log("Error occurred in insert operation");
                });
            },
            update: function (key, values) {
                var updatedData = $.extend({ id: key.id }, values);
                return $.ajax({
                    url: "/api/Orders/",
                    method: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(updatedData)
                }).then(function (response) {
                    if (!response.success) {
                        console.log(response.message);
                        alert(response.message);
                        return;
                    }
                }).fail(function (jqXHR) {
                    console.log("Error occurred in update operation");
                });
            },
            remove: function (key) {
                console.log(key.id);
                return $.ajax({
                    url: "/api/Orders/" + key.id,
                    method: "DELETE"
                }).then(function (response) {
                    if (!response.success) {
                        console.log(response.message);
                        alert(response.message);
                        return;
                    }
                }).fail(function (jqXHR) {
                    console.log("Error occurred in delete operation");
                });
            }
        },
        columns: [
            {
                dataField: "orderDeliveryStatus",
                caption: "Delivery Status",
                validationRules: [{ type: "required", message: "Delivery Status is required." }],
                lookup: {
                    dataSource: [
                        { value: 0, text: "Delivered" },
                        { value: 1, text: "Not Delivered" },
                        { value: 2, text: "Returned" }
                    ],
                    valueExpr: "value",
                    displayExpr: "text"
                },
                cellTemplate: function (container, options) {
                    var icon = "";
                    var text = "";

                    switch (options.value) {
                        case 0: icon = '<i class="fa-solid fa-check" style="color: #63E6BE;"></i>'; break;
                        case 1: icon = '<i class="fa-solid fa-ban" style="color: #FF0000;"></i>'; break;
                        case 2: icon = '<i class="fa-solid fa-rotate-left" style="color: #74C0FC;"></i>'; break;
                    }

                    var content = $("<div>").css({ "text-align": "center", "width": "100%" }).html(icon);
                    container.append(content);
                }
            },
            {
                dataField: "orderStatus",
                caption: "Order Status",
                validationRules: [{ type: "required", message: "Order Status is required." }],
                lookup: {
                    dataSource: [
                        { value: 0, text: "Shipped" },
                        { value: 1, text: "In transit" },
                        { value: 2, text: "Reached" },
                    ],
                    valueExpr: "value",
                    displayExpr: "text"
                },
                cellTemplate: function (container, options) {
                    var icon = "";
                    var text = "";

                    switch (options.value) {
                        case 0: icon = '<i class="fa-solid fa-truck" style="color: #74C0FC;"></i>'; break;
                        case 1: icon = '<i class="fa-regular fa-clock" style="color: #63E6BE;"></i>'; break;
                        case 2: icon = '<i class="fa-solid fa-cube" style="color: #FFD43B;"></i>'; break;
                    }

                    var content = $("<div>").css({ "text-align": "center", "width": "100%" }).html(icon);
                    container.append(content);
                }
            },
            {
                dataField: "invoice",
                caption: "Invoice",
                dataType: "string",
                validationRules: [
                    { type: "required", message: "Invoice number is required." },
                    { type: "range", min: 1, message: "Invoice number must be positive." }
                ]
            },
            { dataField: "orderDate", caption: "Order date", dataType: "date", format: "yyyy-MM-dd", validationRules: [{ type: "required", message: "Order Date is required." }] },
            {
                dataField: "shippedDate",
                caption: "Shipped date",
                dataType: "date",
                format: "yyyy-MM-dd",
                validationRules: [
                    { type: "required", message: "Shipped Date is required." },
                    {
                        type: "custom",
                        validationCallback: function (e) {
                            return new Date(e.data.shippedDate) >= new Date(e.data.orderDate);
                        },
                        message: "Shipped Date must be later than Order Date."
                    }
                ]
            },
            {
                dataField: "company",
                caption: "Company name",
                validationRules: [
                    { type: "required", message: "Company name is required." },
                    { type: "stringLength", max: 100, message: "Company name cannot exceed 100 characters." }
                ]
            },
            {
                dataField: "store",
                caption: "Store",
                validationRules: [
                    { type: "required", message: "Store name is required." },
                    { type: "stringLength", max: 100, message: "Store name cannot exceed 100 characters." }
                ]
            },
            {
                dataField: "orderTotal",
                caption: "Order total",
                dataType: "number",
                format: "$#,##0.00",
                validationRules: [
                    { type: "required", message: "Order total is required." },
                    { type: "range", min: 0, message: "Order Total cannot be negative." }
                ]
            },
            {
                dataField: "paymentTotal",
                caption: "Payment total",
                dataType: "number",
                format: "$#,##0.00",
                validationRules: [
                    { type: "required", message: "Payment total is required." },
                    { type: "range", min: 0, message: "Payment Total cannot be negative." }
                ]
            }
        ],
        onRowUpdating: function (e) {
            e.newData = $.extend({}, e.oldData, e.newData);
        },
        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: "Order Details",
                showTitle: true,
                width: 700,
                height: 525
            }
        },
        masterDetail: {
            enabled: true,
            template: function (container, options) {
                var orderId = options.data.id;
                $("<div>").addClass("detail-grid").appendTo(container).dxDataGrid({
                    dataSource: {
                        load: function () {
                            return $.ajax({
                                url: `/api/products/${orderId}`,
                                dataType: "json"
                            }).then(function (response) {
                                if (!response.success) {
                                    console.log(response.message);
                                    alert(response.message);
                                    return;
                                }
                                return response.data;
                            }).fail(function (jqXHR) {
                                console.log("Error occurred in load operation");
                            });
                        },
                        insert: function (values) {
                            values.orderId = orderId; 
                            return $.ajax({
                                url: "/api/products",
                                method: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(values)
                            }).then(function (response) {
                                if (!response.success) {
                                    console.log(response.message);
                                    alert(response.message);
                                    return;
                                }
                                return response.data;
                            }).fail(function (jqXHR) {
                                console.log("Error occurred in insert operation");
                            });
                        },
                        update: function (key, values) {
                            var updatedData = $.extend({ id: key.id }, values);
                            return $.ajax({
                                url: "/api/products/",
                                method: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(updatedData)
                            }).then(function (response) {
                                if (!response.success) {
                                    console.log(response.message);
                                    alert(response.message);
                                    return;
                                }
                                return response.data;
                            }).fail(function (jqXHR) {
                                console.log("Error occurred in update operation");
                            });
                        },
                        remove: function (key) {
                            return $.ajax({
                                url: `/api/products/${key.productId}`,
                                method: "DELETE"
                            }).then(function (response) {
                                if (!response.success) {
                                    console.log(response.message);
                                    alert(response.message);
                                    return;
                                }
                            }).fail(function (jqXHR) {
                                console.log("Error occurred in delete operation");
                            });
                        }
                    },
                    onRowUpdating: function (e) {
                        e.newData = $.extend({}, e.oldData, e.newData);
                    },
                    keyExpr: "id",
                    columns: [
                        {
                            dataField: "productName",
                            caption: "Product Name",
                            validationRules: [
                                { type: "required", message: "Product Name is required." },
                                { type: "stringLength", max: 100, message: "Product Name cannot exceed 100 characters." }
                            ]
                        },
                        {
                            dataField: "noOfUnits",
                            caption: "Units",
                            validationRules: [
                                { type: "required", message: "Units is required." },
                                { type: "range", min: 1, message: "No. of Units must be positive." }
                            ]
                        },
                        {
                            dataField: "unitPrice",
                            caption: "Price",
                            datatype: "number",
                            format: "$#,##0.00",
                            validationRules: [
                                { type: "required", message: "Unit Price is required." },
                                { type: "range", min: 1, message: "Unit Price must be positive." }
                            ]
                        },
                        {
                            dataField: "discount",
                            caption: "Discount",
                            datatype: "number",
                            format: "$#,##0.00",
                            validationRules: [
                                { type: "required", message: "Discount amount is required." },
                                { type: "range", min: 0, message: "Discount cannot be negative." }
                            ]
                        },
                        {
                            caption: "Total",
                            calculateCellValue: function (data) {
                                return data.noOfUnits * data.unitPrice - data.discount;
                            },
                            datatype: "number",
                            format: "$#,##0.00",
                            formItem: {
                                visible: false 
                            }
                        }
                    ],
                    summary: {
                        totalItems: [
                            {
                                column: "noOfUnits",
                                summaryType: "sum",
                                displayFormat: "SUM={0}"
                            },
                            {
                                column: "unitPrice",
                                summaryType: "avg",
                                displayFormat: "AVG={0}",
                                valueFormat: "$#,##0.00"
                            },
                            {
                                column: "Total",
                                summaryType: "sum",
                                displayFormat: "SUM={0}",
                                valueFormat: "$#,##0.00"
                            }
                        ]
                    },
                    editing: {
                        mode: "popup",
                        allowAdding: true,
                        allowUpdating: true,
                        allowDeleting: true,
                        popup: {
                            title: "Product Details",
                            showTitle: true,
                            width: 700,
                            height: 325
                        }
                    }
                });
            }
        },
        paging: { pageSize: 10 },
        searchPanel: { visible: true },
        groupPanel: { visible: true },
        sorting: { mode: "multiple" },
        allowColumnResizing: true,
    });
});