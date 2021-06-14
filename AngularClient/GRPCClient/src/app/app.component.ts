import { Component, OnInit } from '@angular/core';
import { grpc } from "@improbable-eng/grpc-web";
import { OrderStatus, OrderMessage, Item, OrderResult } from './generated/src/app/protos/OrderProto_pb';
import { OrderProtoService, OrderProtoServiceClient } from './generated/src/app/protos/OrderProto_pb_service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'GRPCClient';
  result: string = "hello world";

  constructor() {
  }

  ngOnInit(): void {
    this.result = "Wait...";
    const orderMsg = new OrderMessage();
    orderMsg.setUserid(2);
    const item1 = new Item();
    item1.setId(2);
    item1.setPrice(30.0);
    item1.setQuantity(5);


    const item2 = new Item();
    item2.setId(3);
    item2.setPrice(40.0);
    item2.setQuantity(3);
    orderMsg.setItemsList([item1, item2]);

    grpc.unary(OrderProtoService.PostOrder, {
      request: orderMsg,
      host: "https://localhost:5001",
      onEnd: res => {
        const { status, message } = res;
        if (status === grpc.Code.OK && message) {
          var orderResult = message.toObject() as OrderResult.AsObject;
          if (orderResult.result == 0)
            this.result = "Order succeeded.";
          else
            this.result = "Order failed.";
        }
        else
          this.result = `Error: ${!message ? "no response" : message}`;
      }
    });
  }



}
//protoc --plugin=protoc-gen-ts="C:\Users\moame\source\repos\gRPCLab1\AngularClient\GRPCClient\node_modules\.bin\protoc-gen-ts.cmd"  --js_out="import_style=commonjs,binary:src/app/generated"  --ts_out="service=grpc-web:src/app/generated" src/app/protos/OrderProto.proto

//protoc --plugin=protoc-gen-ts="C:\Users\moame\source\repos\gRPCLab1\AngularClient\GRPCClient\node_modules\.bin\protoc-gen-ts.cmd"  --js_out="import_style=commonjs,binary:src/app/generated"  --ts_out="service=grpc-web:src/app/generated" src/app/protos/OrderProto.proto


