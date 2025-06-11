<%@ Page Language="vb" Title="" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfAperturaCaja.aspx.vb" Inherits="appWebSistemaRestaurante.wfAperturaCaja" %>

<asp:Content ID="Content100" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container-fluid p-4">
    <div class="panel-heading">
        <h4>Apertura de Caja</h4>
        <button type="button" class="btn btn-success mb-2" onclick="abrirModalNuevo()">+ Nueva Apertura</button>
    </div>
    <div class="panel-body">
        <asp:GridView ID="gvAperturas" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="idArqueo" HeaderText="ID" />
                <asp:BoundField DataField="nombreCajero" HeaderText="Cajero" />
                <asp:BoundField DataField="fechaApertura" HeaderText="Fecha" />
                <asp:BoundField DataField="base" HeaderText="Base S/." DataFormatString="{0:N2}" />
                <asp:BoundField DataField="estado" HeaderText="Estado" />
            </Columns>
        </asp:GridView>
    </div>
</div>

<!-- Modal -->
<div class="modal fade" id="modalApertura" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Apertura de Caja</h5>
                <button type="button" class="close" data-bs-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="ddlCajero">Cajero</label>
                    <asp:DropDownList ID="ddlCajero" runat="server" CssClass="form-control mb-2" />
                </div>

                <div class="form-group">
                    <label for="txtFecha">Fecha y hora</label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>

                <div class="form-group">
                    <label for="txtMoneda">Moneda</label>
                    <asp:TextBox ID="txtMoneda" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>

                <div class="form-group">
                    <label for="txtUsuario">Usuario</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>

                <asp:HiddenField ID="hfIdArqueo" runat="server" />

                <div class="form-group">
                    <label for="txtMontoTotal">Monto Total</label>
                    <asp:TextBox ID="txtMontoTotal" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>

                <div class="form-group">
                    <label for="txtMontoTexto">Monto en Texto</label>
                    <asp:TextBox ID="txtMontoTexto" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                </div>

                <asp:GridView ID="gvDenominaciones" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:BoundField DataField="Denominacion" HeaderText="Denominación" />
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control cantidad-denom" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:N2}" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="modal-footer">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClientClick="return validarCampos();" OnClick="btnGuardar_Click" />
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    const denominaciones = {
        "Billete de S/200": 200,
        "Billete de S/100": 100,
        "Billete de S/50": 50,
        "Billete de S/20": 20,
        "Billete de S/10": 10,
        "Moneda de S/5": 5,
        "Moneda de S/2": 2,
        "Moneda de S/1": 1,
        "Moneda de 0.20": 0.2,
        "Moneda de 0.10": 0.1
    };

    function actualizarTotales() {
        let total = 0;

        const grid = document.getElementById('<%= gvDenominaciones.ClientID %>');
        const rows = grid.getElementsByTagName('tr');

        for (let i = 1; i < rows.length; i++) {
            const row = rows[i];
            const cells = row.getElementsByTagName('td');

            if (cells.length < 3) continue;

            const denominacion = cells[0].innerText.trim();
            const cantidadInput = cells[1].querySelector('input[type="text"]');
            const subtotalCell = cells[2];

            let cantidad = parseInt(cantidadInput.value) || 0;
            let valor = denominaciones[denominacion] || 0;
            let subtotal = cantidad * valor;

            subtotalCell.innerText = subtotal.toFixed(2);
            total += subtotal;
        }

        document.getElementById('<%= txtMontoTotal.ClientID %>').value = total.toFixed(2);
        document.getElementById('<%= txtMontoTexto.ClientID %>').value = 'SOLES'; // O convertir con otra función
    }
    document.addEventListener('DOMContentLoaded', function () {
        $('#modalApertura').on('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                return false;
            }
        });
        const grid = document.getElementById('<%= gvDenominaciones.ClientID %>');
        grid.addEventListener('input', function (e) {
            if (e.target && e.target.tagName === 'INPUT') {
                actualizarTotales();
            }
        });
    });
    function abrirModalNuevo() {
        $('#<%= hfIdArqueo.ClientID %>').val(""); // Nuevo = sin ID
        $('#<%= ddlCajero.ClientID %>').val("");
        $('#<%= txtFecha.ClientID %>').val(new Date().toISOString().slice(0, 16).replace('T', ' '));
        $('#<%= txtMontoTotal.ClientID %>').val("0.00");
        $('#<%= txtMontoTexto.ClientID %>').val("SOLES");

        // Resetear cantidades a 0
        $('#<%= gvDenominaciones.ClientID %> input[type="text"]').val("0");
        $('#<%= gvDenominaciones.ClientID %> td:nth-child(3)').text("0.00");

        $('#modalApertura').modal('show');
    }
    function validarCampos() {
        if (!$('#<%= ddlCajero.ClientID %>').val()) {
            Swal.fire('Falta cajero', 'Debes seleccionar un cajero', 'warning');
            return false;
        }
        return true;
    }   
</script>
</asp:Content>
