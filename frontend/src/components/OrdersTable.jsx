import { useEffect, useState } from "react";
import { GetOrders } from "../services/OrdersService";
import StatusBadge from "./StatusBadge";

export default function OrdersTable() {
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
        <div className="p-5">
            <table className="w-full text-left table-auto hidden md:table border border-slate-200 rounded-md">
                <thead>
                    <tr className="border-b bg-slate-200">
                        <th className="p-2">Id</th>
                        <th className="p-2">Cliente</th>
                        <th className="p-2">Produto</th>
                        <th className="p-2">Valor</th>
                        <th className="p-2">Status</th>
                        <th className="p-2">Data da criacao</th>
                    </tr>
                </thead>
                <tbody>
                    {orders.map((order, index) =>
                        <tr key={index} className={index % 2 != 0 ? "bg-slate-100" : "bg-white"}>
                            <td className="p-3 font-medium">{order.id}</td>
                            <td className="p-3 font-medium">{order.cliente}</td>
                            <td className="p-3 font-medium">{order.produto}</td>
                            <td className="p-3 font-medium">R$ {order.valor}</td>
                            <td className="p-3 font-medium"><StatusBadge status={order.status}></StatusBadge></td>
                            <td className="p-3 font-medium">{new Date(order.data_criacao)
                                .toLocaleString("pt-BR", {
                                    dateStyle: "short"
                                })}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}