<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAperturaCaja.aspx.vb" Inherits="tuProyecto.wfAperturaCaja" %>

<div class="container-fluid p-4">
    <div class="panel-heading">
        <h4>Apertura de Caja</h4>
        <button class="btn btn-success mb-2" onclick="abrirModalNuevo()">+ Nueva Apertura</button>
    </div>
    <div class="panel-body">
        <asp:GridView ID="gvAperturas" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="idArqueo" HeaderText="ID" />
                <asp:BoundField DataField="nombreCajero" HeaderText="Cajero" />
                <asp:BoundField DataField="fechaApertura" HeaderText="Fecha" />
                <asp:BoundField DataField="base" HeaderText="Base S/." DataFormatString="{0:N2}" />
                <asp:BoundField DataField="estado" HeaderText="Estado" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <button class="btn btn-primary btn-sm" onclick='editarArqueo(<%# Eval("idArqueo") %>)'>Editar</button>
                    </ItemTemplate>
                </asp:TemplateField>
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
                <asp:DropDownList ID="ddlCajero" runat="server" CssClass="form-control mb-2" />
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                <asp:TextBox ID="txtMoneda" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                <asp:TextBox ID="txtMontoTotal" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                <asp:TextBox ID="txtMontoTexto" runat="server" CssClass="form-control mb-2" ReadOnly="true" />
                <asp:GridView ID="gvDenominaciones" runat="server" AutoGenerateColumns="False" CssClass="table">
                    <Columns>
                        <asp:BoundField DataField="Denominacion" HeaderText="Denominación" />
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCantidad" runat="server" AutoPostBack="true"
                                    OnTextChanged="txtCantidad_TextChanged" CssClass="form-control" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:N2}" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Text="Guardar" OnClick="btnGuardar_Click" />
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function abrirModalNuevo() {
        $('#modalApertura').modal('show');
    }

    function editarArqueo(id) {
        // Este es un ejemplo; si deseas precargar valores, puedes hacerlo por AJAX + WebMethod
        alert("Funcionalidad de edición en construcción. ID: " + id);
        $('#modalApertura').modal('show');
    }
</script>

