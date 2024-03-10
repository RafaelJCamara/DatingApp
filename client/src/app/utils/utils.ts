export function UpdateListWithObject<T>(objectList: T[], objectToUpdate: T) {
  const index = objectList.indexOf(objectToUpdate);
  objectList[index] = objectToUpdate;
}
