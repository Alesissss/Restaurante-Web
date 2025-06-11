<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plantilla.Master" CodeBehind="wfInicio.aspx.vb" Inherits="appWebSistemaRestaurante.wfInicio" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <style>
        .bienvenida-container {
            margin-top: 60px;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .bienvenida-card {
            width: 100%;
            max-width: 900px;
            border-radius: 12px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);
            overflow: hidden;
            animation: fadeIn 0.6s ease-in-out;
        }

        .bienvenida-header {
            background: linear-gradient(90deg, #007bff, #0056b3);
            padding: 30px;
            color: white;
            text-align: center;
        }

        .bienvenida-header h3 {
            margin: 0;
            font-size: 28px;
            font-weight: 600;
        }

        .bienvenida-body {
            background-color: #f8f9fa;
            padding: 30px;
            text-align: center;
        }

        .bienvenida-body p {
            font-size: 18px;
            line-height: 1.6;
            color: #333;
        }

        .bienvenida-banner {
            max-height: 300px;
            margin-top: 25px;
            border-radius: 8px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }
    </style>

    <section class="content">
        <div class="container-fluid bienvenida-container">
            <div class="bienvenida-card">
                <div class="bienvenida-header">
                    <h3>Bienvenido al Sistema Restaurante</h3>
                </div>
                <div class="bienvenida-body">
                    <p class="lead">
                        Este sistema ha sido diseñado para ayudarte a administrar de manera eficiente cada aspecto de tu restaurante.
                        Desde la gestión de productos, cartas, tipos de usuario y clientes, hasta las operaciones como pedidos y control de caja.
                        También podrás generar reportes para tomar decisiones informadas y mejorar la experiencia de tus comensales.
                    </p>
                    <img src="/bannerSistemaChico.png" alt="Dashboard" class="img-fluid bienvenida-banner" />
                </div>
            </div>
        </div>
    </section>
</asp:Content>
