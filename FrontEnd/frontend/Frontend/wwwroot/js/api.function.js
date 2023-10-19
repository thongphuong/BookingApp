var apiConfig = {
    "api": {
        "host_user_service": USER_SERVICE,
        "host_booking_service": BOOKING_SERVICE,
        "user": {
            "controller": "/user",
            'action': {
                'authenticate': {
                    'method': 'POST',
                    'path': '/authenticate'
                },
                'table_user': {
                    'method': 'GET',
                    'path': '/table-user'
                },
                'add_user': {
                    'method': 'POST',
                    'path': "/add-user"
                },
                'edit_user': {
                    'method': 'POST',
                    'path': "/edit-user"
                },
                'get_user': {
                    'method': 'GET',
                    'path': "/get-user"
                },
                'delete_user': {
                    'method': 'POST',
                    'path': "/delete-user"
                },
                'changeactive_user': {
                    'method': 'POST',
                    'path': "/changeactive-user"
                }
            }
        },
        "menu": {
            "controller": "/menu",
            'action': {
                'get_menu': {
                    'method': 'GET',
                    'path': '/get-menu'
                }
            }
        },
        "department": {
            "controller": "/department",
            'action': {
                'dropdown_department': {
                    'method': 'GET',
                    'path': '/dropdown-department'
                },
                'dropdown_division': {
                    'method': 'GET',
                    'path': '/dropdown-division'
                },
            }
        },
        "store": {
            "controller": "/Store",
            'action': {
                'dropdown_store': {
                    'method': 'GET',
                    'path': '/dropdown-store'
                },
                'edit_store': {
                    'method': 'POST',
                    'path': '/EditData'
                },
                'add_store': {
                    'method': 'POST',
                    'path': '/Add'
                },
                'delete_store': {
                    'method': 'POST',
                    'path': '/Delete'
                },
                'change_store': {
                    'method': 'POST',
                    'path': '/Change'
                },
                'get_edit': {
                    'method': 'GET',
                    'path': '/GetEdit'
                },
                'get_table': {
                    'method': 'GET',
                    'path': '/GetTable'
                },
                'get_product': {
                    'method': 'GET',
                    'path': "/get-store"
                }


            }
        },
        "return": {
            "controller": "/Return",
            'action': {
                'dropdown_return': {
                    'method': 'GET',
                    'path': '/dropdown-return'
                },
                'edit_return': {
                    'method': 'POST',
                    'path': '/Edit'
                },
                'add_return': {
                    'method': 'POST',
                    'path': '/Add'
                },
                'delete_return': {
                    'method': 'POST',
                    'path': '/Delete'
                },
                'change_return': {
                    'method': 'POST',
                    'path': '/Change'
                },
                'get_edit': {
                    'method': 'GET',
                    'path': '/GetEdit'
                },
                'get_table': {
                    'method': 'GET',
                    'path': '/GetTable'
                }
            }
        },
        "role": {
            "controller": "/role",
            'action': {
                'table_role': {
                    'method': 'GET',
                    'path': '/table-role'
                },
                'add_role': {
                    'method': 'POST',
                    'path': "/add-role"
                },
                'edit_role': {
                    'method': 'POST',
                    'path': "/edit-role"
                },
                'dropdown_role': {
                    'method': 'GET',
                    'path': "/dropdown-role"
                },
                'delete_role': {
                    'method': 'POST',
                    'path': "/delete-role"
                },
                'changeactive_role': {
                    'method': 'POST',
                    'path': "/changeactive-role"
                },
                'get_function': {
                    'method': 'GET',
                    'path': "/get-function"
                }
            }
        },

        "supplier": {
            "controller": "/supplier",
            'action': {
                'dropdown_supplier': {
                    'method': 'GET',
                    'path': "/dropdown-supplier"
                },
                'get_supplier': {
                    'method': 'GET',
                    'path': "/get-supplier"
                }
            }
        },
        "line": {
            "controller": "/line",
            'action': {
                'dropdown_line': {
                    'method': 'GET',
                    'path': "/dropdown-line"
                },
                'dropdown_linedepartment': {
                    'method': 'GET',
                    'path': "/dropdown-linedepartment"
                }
            }
        },
        "systemparameter": {
            "controller": "/systemparameter",
            'action': {
                'table_systemparameter': {
                    'method': 'GET',
                    'path': '/table-systemparameter'
                },
                'add_systemparameter': {
                    'method': 'POST',
                    'path': "/add-systemparameter"
                },
                'edit_systemparameter': {
                    'method': 'POST',
                    'path': "/edit-systemparameter"
                },
                'delete_systemparameter': {
                    'method': 'POST',
                    'path': "/delete-systemparameter"
                },
                'changeactive_systemparameter': {
                    'method': 'POST',
                    'path': "/changeactive-systemparameter"
                },
                'get_function': {
                    'method': 'GET',
                    'path': "/get-systemparameter"
                },
                'get_redis': {
                    'method': 'GET',
                    'path': "/get-systemparameter-redis"
                }
            }
        },
        "timeframe": {
            "controller": "/timeframe",
            'action': {
                'dropdown_timeframe': {
                    'method': 'GET',
                    'path': "/dropdown-timeframe"
                },
                'table_timeframe': {
                    'method': 'GET',
                    'path': '/table-timeframe'
                },
                'add_timeframe': {
                    'method': 'POST',
                    'path': "/add-timeframe"
                },
                'edit_timeframe': {
                    'method': 'POST',
                    'path': "/edit-timeframe"
                },
                'delete_timeframe': {
                    'method': 'POST',
                    'path': "/delete-timeframe"
                },
                'changeactive_timeframe': {
                    'method': 'POST',
                    'path': "/changeactive-timeframe"
                },
                'get_function': {
                    'method': 'GET',
                    'path': "/get-timeframe"
                }
            }
        },
        "order": {
            "controller": "/order",
            'action': {
                'add_order': {
                    'method': 'POST',
                    'path': "/add-order"
                },
                'table_order': {
                    'method': 'GET',
                    'path': '/table-order'
                },
                'table_order_refuse': {
                    'method': 'GET',
                    'path': '/table-order-refuse'
                },
                'get_order': {
                    'method': 'GET',
                    'path': '/get-order'
                },
                'delete_order': {
                    'method': 'POST',
                    'path': "/delete-order"
                },
                'edit_order': {
                    'method': 'POST',
                    'path': "/edit-order"
                },
                'approve_order': {
                    'method': 'POST',
                    'path': "/approve-order"
                },
                'return_order': {
                    'method': 'POST',
                    'path': "/return-order"
                },
                'refuse_order': {
                    'method': 'POST',
                    'path': "/refuse-order"
                },
                'export_order': {
                    'method': 'GET',
                    'path': "/export-order"
                },
                'export_order_refuse': {
                    'method': 'GET',
                    'path': "/export-order-refuse"
                },
            }
        },
        "product": {
            "controller": "/product",
            'action': {
                'dropdown_product': {
                    'method': 'GET',
                    'path': "/dropdown-product"
                },
                'get_product': {
                    'method': 'GET',
                    'path': "/get-product"
                },
                'dropdown_product_paging': {
                    'method': 'GET',
                    'path': "/dropdown-product-paging"
                }
            }
        },
    }
}