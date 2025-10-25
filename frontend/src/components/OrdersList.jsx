import { useEffect, useState } from "react";
import { GetOrders } from "../services/OrdersService";
import StatusBadge from "./StatusBadge";

export default function OrdersList() {
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        const intervalId = setInterval(() => {
            GetOrders().then((orders) => {
                setOrders(orders.data);
            });
        }, 1000);
        return () => clearInterval(intervalId);
    }, []);

    return (
        <table className="w-full text-left table-auto">
            <thead>
                <tr className="bg-slate-200">
                    <th className="border-b">Id</th>
                    <th className="border-b">Cliente</th>
                    <th className="border-b">Produto</th>
                    <th className="border-b">Valor</th>
                    <th className="border-b">Status</th>
                    <th className="border-b">Data da criacao</th>
                </tr>
            </thead>
            <tbody>
                {orders.map((order, index) =>
                    <tr key={index} className={index % 2 != 0 ? "bg-slate-100" : "bg-white"}>
                        <td className="p-2">{order.id}</td>
                        <td className="p-2">{order.cliente}</td>
                        <td className="p-2">{order.produto}</td>
                        <td className="p-2">R$ {order.valor}</td>
                        <td className="p-2"><StatusBadge status={order.status}></StatusBadge></td>
                        <td className="p-2">{new Date(order.data_criacao)
                            .toLocaleString("pt-BR", {
                                dateStyle: "short"
                            })}</td>
                    </tr>
                )}
            </tbody>
        </table>
    );
}