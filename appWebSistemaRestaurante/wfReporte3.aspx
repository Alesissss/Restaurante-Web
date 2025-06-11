<%@ Page Title="Análisis de Productos" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfReporte3.aspx.vb" Inherits="appWebSistemaRestaurante.wfReporte3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <section class="content">
        <div class="container-fluid p-4">
            <div class="panel-heading">
                <h1>Reporte de Análisis de Productos</h1>
                <p class="text-muted">Identifica tus productos y categorías más populares y rentables.</p>
            </div>
            <div class="panel-body mt-4">
                <div class="row">
                    <div class="col-md-8">
                        <div class="card">
                            <div class="card-header"><h3 class="card-title">Top 10 Productos Más Vendidos (por Cantidad)</h3></div>
                            <div class="card-body">
                                <div id="chartTopProductos" style="width: 100%; height: 400px;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card">
                            <div class="card-header"><h3 class="card-title">Ventas por Categoría</h3></div>
                            <div class="card-body">
                                <div id="chartVentasCategoria" style="width: 100%; height: 400px;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageSpecificScripts" runat="server">
    <script>
        $(document).ready(function () {
            // Inicializar gráficos
            var chartTopProd = echarts.init(document.getElementById('chartTopProductos'), 'dark');
            var chartCat = echarts.init(document.getElementById('chartVentasCategoria'), 'dark');

            // Cargar datos
            $.ajax({
                type: "POST",
                url: "wfReporte3.aspx/CargarDatosReporte",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    const datos = response.d;
                    
                    // Configurar gráfico de Top Productos (Barras Horizontales)
                    let optionTopProd = {
                        tooltip: { trigger: 'axis', axisPointer: { type: 'shadow' } },
                        grid: { left: '3%', right: '4%', bottom: '3%', containLabel: true },
                        xAxis: { type: 'value', boundaryGap: [0, 0.01] },
                        yAxis: { type: 'category', data: datos.NombresProductos.reverse() }, // reverse para mostrar el top 1 arriba
                        series: [{
                            name: 'Cantidad Vendida', type: 'bar',
                            data: datos.CantidadesVendidas.reverse(),
                            itemStyle: { color: '#91CC75' }
                        }]
                    };
                    chartTopProd.setOption(optionTopProd);

                    // Configurar gráfico de Categorías (Pie/Dona)
                    let optionCat = {
                        tooltip: { trigger: 'item', formatter: '{a} <br/>{b}: S/ {c} ({d}%)' },
                        legend: { orient: 'vertical', left: 'left' },
                        series: [{
                            name: 'Ventas', type: 'pie', radius: '50%',
                            data: datos.VentasPorCategoria,
                            emphasis: { itemStyle: { shadowBlur: 10, shadowOffsetX: 0, shadowColor: 'rgba(0, 0, 0, 0.5)' } }
                        }]
                    };
                    chartCat.setOption(optionCat);
                },
                error: function (xhr) { console.error("Error al cargar datos: " + xhr.responseText); }
            });

            window.addEventListener('resize', function() {
                chartTopProd.resize();
                chartCat.resize();
            });
        });
    </script>
</asp:Content>