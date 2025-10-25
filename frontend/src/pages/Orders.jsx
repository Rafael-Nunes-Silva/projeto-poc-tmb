import OrdersTable from "../components/OrdersTable";
import OrdersList from "../components/OrdersList";

export default function Orders() {
    return (
        <div>
            <div className="hidden md:block">
                <OrdersTable></OrdersTable>
            </div>
            <div className="md:hidden">
                <OrdersList></OrdersList>
            </div>
        </div>
    );
}