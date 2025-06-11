<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfReporte1.aspx.vb" Inherits="appWebSistemaRestaurante.wfReporte1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <section class="content">
        <div class="container-fluid p-4">
            <h2 class="mb-4">Reporte General de Pedidos</h2>

            <!-- Infoboxes -->
            <div class="row text-white mb-4">
                <div class="col-md-4">
                    <div class="bg-primary p-4 rounded shadow">
                        <h4 class="text-center">Pedidos del Día</h4>
                        <h2 class="text-center" id="lblPedidosDia">0</h2>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="bg-success p-4 rounded shadow">
                        <h4 class="text-center">Pedidos Pendientes</h4>
                        <h2 class="text-center" id="lblPedidosPendientes">0</h2>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="bg-warning p-4 rounded shadow">
                        <h4 class="text-center">Producto + Vendido</h4>
                        <h2 class="text-center" id="lblMasVendido">--</h2>
                    </div>
                </div>
            </div>

            <!-- Gráficos -->
            <div class="row">
                <div class="col-md-6 mb-4">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3>Gráfico de pedidos por estado de pago</h3>
                        </div>
                        <div class="card-body">
                            <div id="graficoEstadoPedidos" style="height: 400px;"></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 mb-4">
                    <div class="card card-info">
                        <div class="card-header">
                            <h3>Gráfico de ingresos en los últimos 7 días</h3>
                        </div>
                        <div class="card-body">
                            <div id="graficoVentasDiarias" style="height: 400px;"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
<script>

    $(document).ready(function () {
        let isDarkMode = document.body.classList.contains('dark-mode');

        cargarIndicadores();
        cargarGraficoEstadoPedidos();
        cargarGraficoVentasDiarias();

        function cargarIndicadores() {
            $.ajax({
                type: "POST",
                url: "wfReporte1.aspx/GetIndicadores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    const datos = res.d;
                    $('#lblPedidosDia').text(datos.pedidosDia);
                    $('#lblPedidosPendientes').text(datos.pedidosPendientes);
                    $('#lblMasVendido').text(datos.productoMasVendido);
                },
                error: function () {
                    toastr.error("Error al cargar indicadores");
                }
            });
        }

        function cargarGraficoEstadoPedidos() {
            $.ajax({
                type: "POST",
                url: "wfReporte1.aspx/GetGraficoEstadoPedidos",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    const data = res.d;

                    var chart = echarts.init(document.getElementById('graficoEstadoPedidos'), isDarkMode ? 'dark' : '');
                    var option = {
                        title: { text: 'Estado de Pagos', left: 'center' },
                        tooltip: {
                            trigger: 'item',
                            formatter: '{b}: {c} pedidos ({d}%)'
                        },
                        legend: {
                            bottom: 0
                        },
                        series: [{
                            name: 'Pedidos',
                            type: 'pie',
                            radius: ['40%', '70%'],
                            avoidLabelOverlap: false,
                            label: {
                                show: true,
                                formatter: '{b}\n{c} ({d}%)',
                                fontSize: 14
                            },
                            emphasis: {
                                label: {
                                    show: true,
                                    fontSize: 16,
                                    fontWeight: 'bold'
                                }
                            },
                            data: [
                                { value: data.pagado, name: 'Pagado' },
                                { value: data.pendiente, name: 'Pendiente' }
                            ]
                        }]
                    };
                    chart.setOption(option);
                    window.addEventListener('resize', chart.resize);
                },
                error: function () {
                    toastr.error("Error al cargar gráfico de estado de pagos");
                }
            });
        }

        function cargarGraficoVentasDiarias() {
            $.ajax({
                type: "POST",
                url: "wfReporte1.aspx/GetGraficoVentasDiarias",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    const data = res.d;

                    var chart = echarts.init(document.getElementById('graficoVentasDiarias'), isDarkMode ? 'dark' : '');
                    var option = {
                        title: { text: 'Ventas de la Semana', left: 'center' },
                        tooltip: {
                            trigger: 'axis',
                            formatter: function (params) {
                                let texto = `${params[0].axisValue}<br/>`;
                                params.forEach(p => {
                                    texto += `${p.marker} ${p.seriesName}: S/. ${formatearNumero(p.data)}<br/>`;
                                });
                                return texto;
                            }
                        },
                        xAxis: {
                            type: 'category',
                            data: data.fechas
                        },
                        yAxis: {
                            type: 'value'
                        },
                        series: [{
                            name: 'Ventas',
                            data: data.ventas,
                            type: 'bar',
                            smooth: true,
                            itemStyle: {
                                borderRadius: [4, 4, 0, 0]
                            },
                            label: {
                                show: true,
                                position: 'top',
                                formatter: function (params) {
                                    return 'S/. ' + formatearNumero(params.value);
                                }
                            }
                        }]
                    };
                    chart.setOption(option);
                    window.addEventListener('resize', chart.resize);
                },
                error: function () {
                    toastr.error("Error al cargar gráfico de ventas");
                }
            });
        }
    });

    function formatearNumero(numero) {
        let numeroRedondeado = Math.round(numero * 100) / 100;
        return new Intl.NumberFormat('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(numeroRedondeado);
    }
</script>
</asp:Content>