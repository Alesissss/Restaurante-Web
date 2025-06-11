<%@ Page Title="Reporte de Empleados" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfReporte2.aspx.vb" Inherits="appWebSistemaRestaurante.wfReporte2" %>

<%-- PRIMER BLOQUE DE CONTENIDO (PARA EL HTML PRINCIPAL) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Reporte de Desempeño por Empleado</h1>
                <p class="text-muted">Análisis de ventas y pedidos gestionados por cada mesero.</p>
            </div>
            <div class="panel-body mt-4">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header"><h3 class="card-title">Total Vendido por Empleado</h3></div>
                            <div class="card-body">
                                <div id="chartVentasPorEmpleado" style="width: 100%; height: 400px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content> <%-- <- Cierre del primer bloque --%>

<%-- SEGUNDO BLOQUE DE CONTENIDO (PARA LOS SCRIPTS) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="PageSpecificScripts" runat="server">
    <script>
        $(document).ready(function () {
            // Inicializar el gráfico
            var chartDom = document.getElementById('chartVentasPorEmpleado');
            var myChart = echarts.init(chartDom, 'dark');
            var option;

            // Cargar los datos con AJAX
            $.ajax({
                type: "POST",
                url: "wfReporte2.aspx/CargarDatosReporte",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        const datos = response.d;
                        option = {
                            tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
                            grid: { left: '3%', right: '4%', bottom: '3%', containLabel: true },
                            xAxis: [{ type: 'category', data: datos.NombresEmpleados, axisTick: { alignWithLabel: true } }],
                            yAxis: [{ type: 'value', axisLabel: { formatter: 'S/ {value}' } }],
                            series: [{
                                name: 'Total Vendido',
                                type: 'bar',
                                barWidth: '60%',
                                data: datos.TotalesVenta,
                                itemStyle: { color: '#5470C6' }
                            }]
                        };
                        myChart.setOption(option);
                    }
                },
                error: function (xhr) {
                    console.error("Error al cargar datos del reporte: " + xhr.responseText);
                    toastr.error("No se pudieron cargar los datos para el gráfico.");
                }
            });

            // Ajustar tamaño del gráfico al cambiar tamaño de la ventana
            window.addEventListener('resize', myChart.resize);
        });
    </script>
</asp:Content> <%-- <- Cierre del segundo bloque --%>