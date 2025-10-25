import { useEffect, useState } from "react";

export default function OrdersList() {
    const [orders, setOrders] = useState(null);

    useEffect(() => {
        //TODO:
        //buscar do backend os pedidos
    }, []);

    return (
        <table className="w-full text-left table-auto">
            <thead>
                <tr>
                    <th className="border-b bg-slate-50">Id</th>
                    <th className="border-b bg-slate-50">Cliente</th>
                    <th className="border-b bg-slate-50">Produto</th>
                    <th className="border-b bg-slate-50">Valor</th>
                    <th className="border-b bg-slate-50">Status</th>
                    <th className="border-b bg-slate-50">Data da criacao</th>
                </tr>
            </thead>
            <tbody>
                {orders.map((order) =>
                    <tr>
                        <td className="p-2">{order.id}</td>
                        <td className="p-2">{order.cliente}</td>
                        <td className="p-2">{order.produto}</td>
                        <td className="p-2">R$ {order.valor}</td>
                        <td className="p-2">{order.status}</td>
                        <td className="p-2">{order.data_criacao}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}