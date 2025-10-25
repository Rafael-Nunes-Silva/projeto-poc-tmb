export default function StatusBadge(props) {
    switch (props.status) {
        case 0:
            return <span className="p-2 bg-yellow-200 border border-slate-200 rounded-md">Pendente</span>;
        case 1:
            return <span className="p-2 bg-blue-200 border border-slate-200 rounded-md">Processando</span>;
        case 2:
            return <span className="p-2 bg-green-200 border border-slate-200 rounded-md">Finalizado</span>;
    }
    return <span className="p-2 bg-red-200 border border-slate-200 rounded-md">Indefinido</span>;
}