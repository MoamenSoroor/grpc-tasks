// package: 
// file: src/app/protos/OrderProto.proto

import * as jspb from "google-protobuf";

export class OrderMessage extends jspb.Message {
  getUserid(): number;
  setUserid(value: number): void;

  clearItemsList(): void;
  getItemsList(): Array<Item>;
  setItemsList(value: Array<Item>): void;
  addItems(value?: Item, index?: number): Item;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): OrderMessage.AsObject;
  static toObject(includeInstance: boolean, msg: OrderMessage): OrderMessage.AsObject;
  static extensions: {[key: number]: jspb.ExtensionFieldInfo<jspb.Message>};
  static extensionsBinary: {[key: number]: jspb.ExtensionFieldBinaryInfo<jspb.Message>};
  static serializeBinaryToWriter(message: OrderMessage, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): OrderMessage;
  static deserializeBinaryFromReader(message: OrderMessage, reader: jspb.BinaryReader): OrderMessage;
}

export namespace OrderMessage {
  export type AsObject = {
    userid: number,
    itemsList: Array<Item.AsObject>,
  }
}

export class Item extends jspb.Message {
  getId(): number;
  setId(value: number): void;

  getQuantity(): number;
  setQuantity(value: number): void;

  getPrice(): number;
  setPrice(value: number): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Item.AsObject;
  static toObject(includeInstance: boolean, msg: Item): Item.AsObject;
  static extensions: {[key: number]: jspb.ExtensionFieldInfo<jspb.Message>};
  static extensionsBinary: {[key: number]: jspb.ExtensionFieldBinaryInfo<jspb.Message>};
  static serializeBinaryToWriter(message: Item, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Item;
  static deserializeBinaryFromReader(message: Item, reader: jspb.BinaryReader): Item;
}

export namespace Item {
  export type AsObject = {
    id: number,
    quantity: number,
    price: number,
  }
}

export class OrderResult extends jspb.Message {
  getResult(): OrderStatusMap[keyof OrderStatusMap];
  setResult(value: OrderStatusMap[keyof OrderStatusMap]): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): OrderResult.AsObject;
  static toObject(includeInstance: boolean, msg: OrderResult): OrderResult.AsObject;
  static extensions: {[key: number]: jspb.ExtensionFieldInfo<jspb.Message>};
  static extensionsBinary: {[key: number]: jspb.ExtensionFieldBinaryInfo<jspb.Message>};
  static serializeBinaryToWriter(message: OrderResult, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): OrderResult;
  static deserializeBinaryFromReader(message: OrderResult, reader: jspb.BinaryReader): OrderResult;
}

export namespace OrderResult {
  export type AsObject = {
    result: OrderStatusMap[keyof OrderStatusMap],
  }
}

export interface OrderStatusMap {
  SUCCESS: 0;
  FAILED: 1;
}

export const OrderStatus: OrderStatusMap;

