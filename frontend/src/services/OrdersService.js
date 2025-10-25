
import axios from 'axios';

export function GetOrders() {
    return axios.get(`${import.meta.env.VITE_BASE_API_URL}orders`);
}