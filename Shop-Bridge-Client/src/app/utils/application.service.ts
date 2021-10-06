import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {

  private http: HttpClient;
  private baseUrl: string;

  constructor(@Inject(HttpClient) http: HttpClient) { 
    this.http = http;
    this.baseUrl = "https://localhost:44374/"
  }

  getItems(pageno: number | null | undefined, pageRecordCount: number | null | undefined, filters: FilterAC[] | null | undefined, filter: string | null | undefined){
    let url = this.baseUrl + "product?";
    if(pageno !== undefined && pageno !== null){
      url += "PageNo="+encodeURIComponent("" + pageno) + "&";
    }
    if (pageRecordCount !== undefined && pageRecordCount !== null){
      url += "PageRecordCount=" + encodeURIComponent("" + pageRecordCount) +"&";
    }
    if (filters !== undefined && filters !== null){
      filters && filters.forEach((item, index) => {
        for (let attr in item){
          if (item.hasOwnProperty(attr)) {
            url += "Filters[" + index + "]." + attr + "=" + encodeURIComponent("" + (<any>item)[attr]) + "&";
          }
        }
      });
    }
    if (filter !== undefined && filter !== null && filter !== ""){
      url += "Filter=" + encodeURIComponent("" + filter)
    }
    return this.http.get(url);
  }

  // getItems(){
  //   return this.http.get(this.baseUrl+"product").
  //     pipe(
  //       map((data:any)=>{
  //         return data;
  //       }), catchError(error =>{
  //         return throwError( 'Something went wrong!' );
  //       })
  //     );
  // }

  getItem(id:string){
    return this.http.get(this.baseUrl+"product/"+id).
      pipe(
        map((data:any)=>{
          return data;
        }), catchError(error =>{
          return throwError( 'Something went wrong!' );
        })
      );
  }

  addItem(product: ProductAC){
    return this.http.post(this.baseUrl+"product", product);
  }

  deleteItem(productId: string){
    return this.http.delete(this.baseUrl+"product/"+productId);
  }

  updateItem(product:ProductAC){
    return this.http.put(this.baseUrl+"product/"+product.id, product);
  }
}

export class ProductAC{
  id?: string | undefined;
  name?: string | undefined;
  description?: string | undefined;
  price?: string | undefined;
}

export class PagedProductAC {
  totalProductsCount?: number;
  products: any;
}

export class FilterAC{
    field?: string | undefined;
    value?: string | undefined;
    operator?: string | undefined;
}
