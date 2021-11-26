Chubb = window.Chubb || {};

Chubb.Product = (function ($, window, document, navigator, localStorage, sessionStorage, undefined) {

    var defaults = null;


    const swalBootstrap = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    var npMainFunction = function (args) {

        defaults = args;

        Initialize_Application(defaults);
        Initialization_Events(defaults);

    }


    function Initialize_Application(defaults) {
        validateProduct
        $("#hdnProductId").val('');
        loadProducts();
        populateCategories();
    }

    function Initialization_Events(defaults) {

        try {
            $("#btnSearchProduct").click(function () {
                loadProducts();
            });
        } catch (e) {
            alert("Element:btnNumberAreas Event:click \nException: " + e.message);
        }

        try {
            $('#productNav').click(function () {
                $("#hdnProductId").val(null);
                $("#productForm").trigger("reset");
                $("#titleProduct").text("Agregar Producto")

            });
        } catch (e) {
            alert("Element:btnProductNav Event:click \nException: " + e.message);
        }

        try {
            $("#btnSaveProduct").click(function () {
                saveProduct();
            });
        } catch (e) {
            alert("Element:btnSaveProduct Event:click \nException: " + e.message);
        }


    }

    ////#region Functions Initialization_Events

    function loadProducts() {
        try {

            var settings = {
                url: defaults.UrlWepApi + "api/Product?searchText=" + $("#inputSearchText").val(),
                async: true,
                type: 'GET',
                dataType: 'json'
            };


            $.ajax(settings).done(function (response) {
                if (response.isSuccess) {
                    $('#tableProducts').DataTable({
                        data: response.data,
                        dom: 'Bfrtip',
                        bAutoWidth: false,
                        responsive: true,
                        paging: true,
                        searching: false,
                        destroy: true,
                        columnDefs: [
                            {
                                "className": "text-center",
                                "targets": "_all"
                            }],
                        columns:
                            [
                                {
                                    "data": "name"
                                }, {
                                    "data": "description"
                                }, {
                                    "data": "unitPrice"
                                }, {
                                    "data": "categoryName"
                                }, {
                                    "data": "id", "render": function (data, type, row) {
                                        return '<button type="button" class="btn btn-danger btn-sm" title="Eliminar" onclick=Chubb.Product.productDelete(' + data + ')><span class="glyphicon glyphicon-trash"></span></button> ' +
                                            '<button type="button" class="btn btn-info btn-sm" title="Editar" onclick=Chubb.Product.productEdit(' + data + ')><span class="glyphicon glyphicon-edit"></span></button> ';
                                    }
                                }

                            ]
                    });

                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message
                    });

                }

                /*LoadingHider('searchForm');*/
            }).fail(function (jqXHR, textStatus, err) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...' + err,
                    text: textStatus
                });
                /*LoadingHider('searchForm');*/
            });

        } catch (e) {
            alert("Method: loadProducts \nException: " + e.message);
        }
    }

    function populateCategories() {
        try {

            var settings = {
                url: defaults.UrlWepApi + "api/Category",
                async: true,
                type: 'GET',
                dataType: 'json'
            };

            $("#inputCategoryId").empty();
            $.ajax(settings).done(function (response) {
                if (response.isSuccess) {

                    $("#inputCategoryId").append('<option value="">Selecciona una opción...</option>');
                    $.each(response.data, function (index, value) {
                        $("#inputCategoryId").append('<option value="' + value.id + '">' + value.name + '</option>');
                    });

                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message
                    });

                }

            }).fail(function (jqXHR, textStatus, err) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...' + err,
                    text: textStatus
                });
            });

        } catch (e) {
            alert("Method: populateCategories \nException: " + e.message);
        }
    }

    function productDelete(id) {
        try {

            swalBootstrap.fire({
                title: "¿Está seguro que desea eliminar el producto?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: "Aceptar",
                cancelButtonText: "Cancelar",

            }).then((result) => {
                if (result.isConfirmed) {
                    var settings = {
                        url: defaults.UrlWepApi + "api/Product/" + id,
                        async: true,
                        type: 'DELETE',
                        dataType: 'json'
                    };

                    $.ajax(settings).done(function (response) {
                        if (response.isSuccess) {

                            Swal.fire({
                                title: "Se ha eliminado correctamente el producto",
                                icon: 'success',
                                showConfirmButton: false,
                                timer: 1500
                            });
                            loadProducts();
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: response.message
                            });

                        }

                    }).fail(function (jqXHR, textStatus, err) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...' + err,
                            text: textStatus
                        });
                    });

                }
            })




        } catch (e) {
            alert("Method: update \nException: " + e.message);
        }
    }

    function productEdit(id) {
        try {

            var settings = {
                url: defaults.UrlWepApi + "api/Product/" + id,
                async: true,
                type: 'GET',
                dataType: 'json'
            };

            $.ajax(settings).done(function (response) {

                console.log(response);
                if (response.isSuccess) {
                    var product = response.data;
                    $('.nav-tabs a[href="#productTab"]').tab('show');
                    $("#hdnProductId").val(product.id);


                    $('#inputName').val(product.name);
                    $('#inputDescription').val(product.description);
                    $('#inputCategoryId').val(product.categoryId);
                    $('#inputUnitPrice').val(product.unitPrice);

                    $("#titleProduct").text("Actualizar Producto");

                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message
                    });

                }

            }).fail(function (jqXHR, textStatus, err) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...' + err,
                    text: textStatus
                });
            });



        } catch (e) {
            alert("Method: update \nException: " + e.message);
        }
    }

    function saveProduct() {
        if ($("#productForm").valid()) {

            var productId = $("#hdnProductId").val();

            if (productId == '' || productId == null) {

                var product = {
                    Name: $('#inputName').val(),
                    Description: $('#inputDescription').val(),
                    UnitPrice: $('#inputUnitPrice').val(),
                    CategoryId: $('#inputCategoryId').val()
                };

                var settings = {
                    url: defaults.UrlWepApi + "api/Product",
                    async: true,
                    data: JSON.stringify(product),
                    type: 'POST',
                    contentType: "application/json",
                    dataType: 'json'
                };


                $.ajax(settings).done(function (response) {

                    if (response.isSuccess) {
                        Swal.fire({
                            title: "Se ha agregado correctamente el producto",
                            icon: 'success',
                            showConfirmButton: false,
                            timer: 1500
                        });

                        $('.nav-tabs a[href="#searchTab"]').tab('show');
                        loadProducts();

                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: response.message
                        });

                    }

                }).fail(function (jqXHR, textStatus, err) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...' + err,
                        text: textStatus
                    });
                });
            }
            else {
                var product = {
                    Name: $('#inputName').val(),
                    Description: $('#inputDescription').val(),
                    UnitPrice: $('#inputUnitPrice').val(),
                    CategoryId: $('#inputCategoryId').val()
                };

                var settings = {
                    url: defaults.UrlWepApi + "api/Product/" + productId,
                    async: true,
                    data: JSON.stringify(product),
                    type: 'PUT',
                    contentType: "application/json",
                    dataType: 'json'
                };


                $.ajax(settings).done(function (response) {

                    if (response.isSuccess) {
                        Swal.fire({
                            title: "Se ha agregado correctamente el producto",
                            icon: 'success',
                            showConfirmButton: false,
                            timer: 1500
                        });

                        $('.nav-tabs a[href="#searchTab"]').tab('show');
                        loadProducts();

                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: response.message
                        });

                    }

                }).fail(function (jqXHR, textStatus, err) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...' + err,
                        text: textStatus
                    });
                });
            }
        }
    }

    function validateProduct() {
        $("#productForm").validate({
            rules: {
                Name: {
                    required: true,
                    maxlength: 50
                },
                Description: {
                    required: true,
                    maxlength: 50,
                },
                CategoryId: {
                    required: true,

                },
                UnitPrice: {
                    required: true,
                    number: true
                }
            }
        });
    }


    //#endregion



    return {
        Config: defaults,
        npMainFunction: npMainFunction,
        productDelete: productDelete,
        productEdit: productEdit
    }
}(jQuery, window, document, navigator, localStorage, sessionStorage, undefined));


