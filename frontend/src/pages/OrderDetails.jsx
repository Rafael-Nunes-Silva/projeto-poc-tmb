import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { GetOrder } from "../services/OrdersService";
import StatusBadge from "../components/StatusBadge";

export default function OrderDetails() {
    const { id } = useParams();
    const [order, setOrder] = useState();

    useEffect(() => {
        const intervalId = setInterval(() => {
            GetOrder(id).then(response => {
                setOrder(response.data);
            });
        }, 1000);
        return () => clearInterval(intervalId);
    }, []);

    return (
        <div className="mx-auto mt-25">
            <div className="max-w-sm mx-auto border border-slate-200 px-5 py-2 bg-slate-100">
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="clienteInput" className="font-semibold">Id</label>
                    <span className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6">
                        {order ? order.id : ""}
                    </span>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="clienteInput" className="font-semibold">Cliente</label>
                    <span className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6">
                        {order ? order.cliente : ""}
                    </span>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="produtoInput" className="font-semibold">Produto</label>
                    <span className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6">
                        {order ? order.produto : ""}
                    </span>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="valorInput" className="font-semibold">Valor</label>
                    <div className="bg-white flex flex-row items-center">
                        <span className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6">
                            R$ {order ? order.valor : ""}
                        </span>
                    </div>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="valorInput" className="font-semibold">Status</label>
                    <div className="flex flex-row items-center">
                        <StatusBadge status={order ? order.status : 0}></StatusBadge>
                    </div>
                </div>
                <div className="my-3 max-w flex flex-col">
                    <label htmlFor="valorInput" className="font-semibold">Data de Criação</label>
                    <div className="bg-white flex flex-row items-center">
                        <span className="bg-white border-b border-slate-300 focus:border-slate-900 block min-w-0 grow py-1.5 pr-3 pl-1 text-base text-slate-900 placeholder:text-slate-400 focus:outline-none sm:text-sm/6">
                            {new Date(order ? order.data_criacao : "")
                                .toLocaleString("pt-BR", {
                                    dateStyle: "short"
                                }) || ""}
                        </span>
                    </div>
                </div>
                <div className="my-2 max-w text-center">
                    <button type="submit" className="p-2 border border-slate-200 rounded-xl bg-blue-200 cursor-pointer">Fazer pedido</button>
                </div>
            </div>
        </div>
    );
}