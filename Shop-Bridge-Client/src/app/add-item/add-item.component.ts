import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApplicationService, ProductAC } from '../utils/application.service';

@Component({
  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss']
})
export class AddItemComponent implements OnInit, OnDestroy {

  product: ProductAC = new ProductAC();
  productId:string = "";
  pageTitle: string = "";

  constructor(private readonly applicationService: ApplicationService, 
            private readonly toastrService: ToastrService,
            private readonly router: Router,
            private readonly route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.productId = params['id'] //log the value of id
   });
  }

  ngOnInit(): void {
    if(this.productId === "" || this.productId === undefined){
      this.pageTitle = "Add new Item";
    }
    else{
      this.pageTitle = "Update Item details";
      this.getItem();
    }
  }

  ngOnDestroy(){
    
  }

  getItem(){
    this.applicationService.getItem(this.productId).subscribe(
      (data:any) =>{
        if(data!=null){
          this.product = data;
        }
      }
    )
  }

  save(){
    if(this.productId === "" || this.productId === undefined){
      this.applicationService.addItem(this.product).subscribe(
        (data:any)=>{
          if(data !== null && data !== undefined){
            this.toastrService.success("Item added successfully");
            this.router.navigate(["item/list"]);
          }
        }, (error)=>{
          this.toastrService.error(error.message);
        }
      );
    }
    else{
      this.applicationService.updateItem(this.product).subscribe(
        (data:any)=>{
          if(data !== null && data !== undefined){
            this.toastrService.success("Item details updated successfully");
            this.router.navigate(["item/list"]);
          }
        }, (err)=>{
          this.toastrService.error(err.message);
        }
      )
    }
  }

}
