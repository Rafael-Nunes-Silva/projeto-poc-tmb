import { useEffect, useState } from "react";
import { GetOrders } from "../services/OrdersService";
import StatusBadge from "./StatusBadge";
import { useNavigate } from 'react-router-dom';
import Loading from "../components/Loading";

export default function OrdersList() {
    const navigate = useNavigate();

    const [orders, setOrders] = useState();

    useEffect(() => {
        const intervalId = setInterval(() => {
            GetOrders().then((response) => {
                console.log(response.data);
                setOrders(response.data);
            });
        }, 1000);
        return () => clearInterval(intervalId);
    }, []);

    return (
        <div className="p-6 space-y-5">
            {
                !orders ? <Loading></Loading> :
                    orders.map((order) => (
                        <div
                            key={order.id}
                            className="bg-slate-50 border border-slate-200 shadow-sm rounded-lg px-5 py-4 cursor-pointer"
                            onClick={() => navigate(`/detalhes-pedido/${order.id}`)}
                        >
                            <div className="flex justify-between items-center">
                                <div className="flex flex-wrap items-center text-slate-700 text-sm sm:text-base font-medium gap-2">
                                    <span className="text-slate-400 font-semibold">{order.id}</span>
                                    <span className="text-slate-300">|</span>
                                    <span className="font-semibold text-lg text-slate-800">{order.produto}</span>
                                    <span className="text-slate-300">|</span>
                                    <span className="text-slate-500">{order.cliente}</span>
                                </div>
                                <span className="text-slate-500 font-medium text-sm">
                                    {new Date(order.data_criacao).toLocaleDateString("pt-BR")}
                                </span>
                            </div>

                            <div className="flex justify-between items-center mt-3">
                                <span className="text-xl font-semibold text-slate-800">
                                    R$ {order.valor}
                                </span>
                                <StatusBadge status={order.status} />
                            </div>
                        </div>
                    ))
            }
        </div>
    );
}