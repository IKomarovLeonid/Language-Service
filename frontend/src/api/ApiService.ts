import {UsersClient, WordsClient} from "./ApiClient";

export class ApiService{
  public readonly words: WordsClient;
  private readonly users: UsersClient;

  constructor() {
    this.words = new WordsClient();
    this.users = new UsersClient();
  }
}
