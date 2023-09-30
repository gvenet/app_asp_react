// ProductModel.ts

export interface Product {
  id: number;
  label?: string | null;
  price: number;
  description?: string | null;
  image_Url?: string | null;
  version: number;
  category?: string | null;
}
