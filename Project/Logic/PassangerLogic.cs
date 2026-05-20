public static class PassangerLogic
{
    public static PassangerModel? GetById(int id)
    {
        PassangerAccess db = new();
        return db.GetById(id);
    }
}
