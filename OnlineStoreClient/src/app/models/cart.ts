import { Product } from "./product";

export class Cart{
    userId: string;
    products: Product[];
    summaryPrice: number;
}