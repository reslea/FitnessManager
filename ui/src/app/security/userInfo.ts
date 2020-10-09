import {PermissionType} from './permissionType';


export default class UserInfo {
  username: string;
  role: string;
  issuedAt: Date;
  expires: Date;
  permissions: PermissionType[];

  constructor(decoded: any) {
    this.permissions = decoded.Permission.map(n => PermissionType[parseInt(n, 0)]);
    this.username = decoded.unique_name;
    this.role = decoded.role;
    this.issuedAt = new Date(decoded.iat * 1000);
    this.expires = new Date(decoded.exp * 1000);
    console.log(this);
  }
}
