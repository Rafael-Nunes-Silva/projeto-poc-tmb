
import axios from 'axios';

export function NewOrder(cliente, produto, valor) {
    return axios.post(`${import.meta.env.VITE_BASE_API_URL}orders`, {
        "Cliente": cliente,
        "Produto": produto,
        "Valor": valor
    });
}

export function GetOrders() {
    return axios.get(`${import.meta.env.VITE_BASE_API_URL}orders`);
}

export function GetOrder(id) {
    return axios.get(`${import.meta.env.VITE_BASE_API_URL}orders/${id}`);
}